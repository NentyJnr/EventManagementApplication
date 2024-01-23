using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Extensions;
using NCSEvent.API.Commons.Models;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;
using PayStack.Net;

namespace NCSEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string token;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITagManagementService _tagManagementService;
        private PayStackApi Paystack { get; set; }

        public PaymentController(IConfiguration configuration, AppDbContext context, IHttpContextAccessor contextAccessor, ITagManagementService tagManagementService)
        {
            _configuration = configuration;
            token = _configuration["Payment:PaystackSK"];
            Paystack = new PayStackApi(token);
            _context = context;
            _contextAccessor = contextAccessor;
            _tagManagementService = tagManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(PaymentViewModel payment)
        {
            var responses = new ServerResponse<PaystackDTO>();
            TransactionInitializeRequest request = new()
            {
                AmountInKobo = (int) ( payment.TotalAmount * 100 ),
                Email = payment.Email,
                Reference = Generate().ToString(),
                Currency = "NGN",
                CallbackUrl = Url.Action("Verify", "Payment", new { RegistrationFormId = payment.RegistrationFormId })
        };

            TransactionInitializeResponse response = Paystack.Transactions.Initialize(request);

            if (response.Status)
            {
                var transaction = new TransactionModel()
                {
                    Name = payment.Email,
                    Amount = payment.TotalAmount,
                    Email = payment.Email,
                    TransactionRef = request.Reference,
                    Status = true
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                var PaystackDTO = new PaystackDTO
                {
                    Reference = request.Reference,
                    RegistrationFormId = payment.RegistrationFormId,
                    AuthorizationUrl = response.Data.AuthorizationUrl
                };

                responses.IsSuccessful = true;
                responses.Data = PaystackDTO;
                responses.SuccessMessage = "Payment Initialized Successful";

                return Ok(responses);
            }

            return BadRequest(response.Message);
        }


        //[HttpGet]
        //public async Task<IActionResult> GetAllPayments()
        //{
        //    var transactions =  _context.Transactions.Where(x => x.Status == true).ToList();

        //    return Ok(transactions);
        //}

        [HttpGet("Verify")]
        public async Task<IActionResult> Verify(string reference, long registrationFormId)
        {
            TransactionVerifyResponse response = Paystack.Transactions.Verify(reference);
            if (response.Data.Status == "success")
            {
                RegistrationForm guest = await _context.RegistrationForms.FirstOrDefaultAsync(g => g.Id == registrationFormId);
                var events = await _context.Events.FirstOrDefaultAsync(e => e.Id == guest.EventManagementId);
                var upload = await _context.Uploads.FirstOrDefaultAsync();

                if (guest != null)
                {
                    guest.PaymentConfirmed = true;

                    _context.RegistrationForms.Update(guest);
                    await _context.SaveChangesAsync();

                    var transaction = _context.Transactions.FirstOrDefault(x => x.TransactionRef == reference);
                    if (transaction != null)
                    {
                        transaction.Status = true;
                        _context.Transactions.Update(transaction);
                        await _context.SaveChangesAsync();

                        var tagDto = new TagDto
                        {
                            EventId = guest.EventManagementId,
                            Email = guest.Email,
                            IsPaymentSuccessful = guest.PaymentConfirmed
                        };

                        ServerResponse<TagModelResponse> tagDetails = await _tagManagementService.GenerateTag(tagDto);

                        return Ok(tagDetails);
                    }
                }
            }
            return BadRequest(new { Error = response.Message });
        }

        public static int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}
