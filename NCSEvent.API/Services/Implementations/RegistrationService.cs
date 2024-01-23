using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Extensions;
using NCSEvent.API.Commons.Models;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Services.Implementations
{
    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IUploadImageHelper _uploadImageHelper;
        private readonly string _uploadUrl;

        public RegistrationService(AppDbContext appDbContext, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IUploadImageHelper uploadImageHelper)
        {
            _context = appDbContext;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _uploadUrl = _configuration["ApplicationSettings:UploadUrl"];
            _uploadImageHelper = uploadImageHelper;
        }

        public async Task<ServerResponse<FormDTO>> IsMemberRegistered(VerifyMembershipNoDTO request)
        {
            var response = new ServerResponse<FormDTO>();

            var existingMember = await _context.UploadMembers.FirstOrDefaultAsync(m => m.MemberShipCode == request.MemberShipCode);

            if (existingMember == null)
            {
                response.IsSuccessful = false;
                response.Data = null;
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                    ResponseDescription = "No User Found"
                };

                return response;
            }

            FormDTO formDTO = existingMember.Adapt<FormDTO>();
            formDTO.EventManagementId = request.EventManagementId;

            formDTO.IsMember = true;

            response.IsSuccessful = true;
            response.Data = formDTO;
            response.SuccessMessage = "Member Exists";

            return response;
        }

        public async Task<ServerResponse<PaymentSummaryDTO>> Register(FormDTO request)
        {
            var response = new ServerResponse<PaymentSummaryDTO>();

            var isRegistered = await _context.RegistrationForms.FirstOrDefaultAsync(r => r.Email == request.Email && r.EventManagementId == request.EventManagementId);
            if (isRegistered != null)
            {
                response.IsSuccessful = false;
                response.Data = null;
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_EXISTS,
                    ResponseDescription = "User Already Exist"
                };

                return response;
            }

            RegistrationForm newForm = request.Adapt<RegistrationForm>();

            // Handle Passport Upload Logic
            string passportFileName = await _uploadImageHelper.UploadImage(request.UploadPassport);
            newForm.PassportUrl = passportFileName; 

            _context.RegistrationForms.Add(newForm);
            await _context.SaveChangesAsync();

            decimal EventAmount;

            if (request.IsMember == true)
            {
                EventAmount = _context.MembershipTypes.FirstOrDefault(m => m.Name == "Member" && m.EventId == request.EventManagementId).Amount;
            }
            else
            {
                EventAmount = _context.MembershipTypes.FirstOrDefault(m => m.Name == "Non-Member" && m.EventId == request.EventManagementId).Amount;
            }

            var Hotel = _context.Hotels.FirstOrDefault(h => h.Id == request.HotelId);
            PaymentSummaryDTO paymentSummaryDTO;
            if (Hotel != null)
            {
                paymentSummaryDTO = new PaymentSummaryDTO
                {
                    RegistrationFormId = newForm.Id,
                    Email = request.Email,
                    EventAmount = EventAmount,
                    HotelAmount = Hotel.Amount,
                    TotalAmount = Hotel.Amount + EventAmount
                };
            }
            else
            {
                paymentSummaryDTO = new PaymentSummaryDTO
                {
                    RegistrationFormId = newForm.Id,
                    Email = request.Email,
                    EventAmount = EventAmount,
                    HotelAmount = 0,
                    TotalAmount = EventAmount
                };
            };

            response.IsSuccessful = true;
            response.Data = paymentSummaryDTO;
            response.SuccessMessage = "User Registered Successfully";

            return response;
        }

        public async Task<ServerResponse<List<RegistrationForm>>> GetAllGuests(int id)
        {
            var response = new ServerResponse<List<RegistrationForm>>();

            try
            {
                var record = await _context.RegistrationForms
                    .Where(e => e.EventManagementId == id)
                    .Select(e => new RegistrationForm
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Email = e.Email,
                        PhoneNumber = e.PhoneNumber,
                        PassportUrl = e.PassportUrl,
                        EventManagementId = e.EventManagementId,
                        Events = e.Events
                    })
                    .ToListAsync();

                if (record == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Event not found."
                    };

                    return response;
                }

                response.Data = record;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = $"An error occurred while retrieving the event: {ex.Message}"
                };
            }

            return response;
        }
    }
}



