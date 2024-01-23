using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;
using System.Security.Cryptography;

namespace NCSEvent.API.Services.Implementations
{
    public class TagManagementService : ITagManagementService
    {
        private readonly AppDbContext _dbContext;

        private readonly ILogger<TagManagementService> _logger;

        public TagManagementService(AppDbContext dbContext, ILogger<TagManagementService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<ServerResponse<TagModelResponse>> GenerateTag(TagDto request)
        {
            var response = new ServerResponse<TagModelResponse>();

            var userDetails = await _dbContext.RegistrationForms.FirstOrDefaultAsync(x => x.Email == request.Email && x.EventManagementId == request.EventId);
            if (userDetails == null)
            {
                response.IsSuccessful = false;
                response.SuccessMessage = "No User found";

                return response;
            }
            if (userDetails.PaymentConfirmed == false)
            {
                response.IsSuccessful = false;
                response.SuccessMessage = "payment is not successful; Tag could not be generated";

                return response;
            }

            var eventId = userDetails.EventManagementId;
            var getEvent = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == eventId);
            
            var uploadTagDetails = await _dbContext.Uploads.FirstOrDefaultAsync();
            if(uploadTagDetails == null)
            {
                response.IsSuccessful = false;
                response.SuccessMessage = "No Tag upload found";

                return response;
            }

            var uniqueId = GeneratePassword(9).ToUpper();
            var res = new TagModelResponse
            {
                LogoUrl = uploadTagDetails.LogoUrl,
                SignatureUrl = uploadTagDetails.SignatureUrl,
                EventStartDate = getEvent.StartDate,
                EventEndDate = getEvent.EndDate,
                EventName = getEvent.Name,
                ProfilePictureUrl = userDetails.PassportUrl,
                FullName = $"{userDetails.FirstName} {userDetails.LastName}",
                uniqueId = uniqueId
            };

            if(res == null)
            {
                response.IsSuccessful = false;
                response.SuccessMessage = "Something went wrong Tag could not be generated";
            }

            response.IsSuccessful = true;
            response.SuccessMessage = "Payment verified successfully";
            response.Data = res;
            response.Error = null;

            return response;
        }



        private static string GeneratePassword(int length)
        {
            string _Chars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ1234567890";
            Byte[] randomBytes = new Byte[length];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            var chars = new char[length + 2]; // Increased length to accommodate spaces

            int Count = _Chars.Length;

            for (int i = 0, j = 0; i < length; i++)
            {
                if (i > 0 && i % 3 == 0)
                {
                    chars[j++] = ' '; // Insert space every 3 characters
                }
                chars[j++] = _Chars[(int)randomBytes[i] % Count];
            }

            return new string(chars);
        }

    }
}
