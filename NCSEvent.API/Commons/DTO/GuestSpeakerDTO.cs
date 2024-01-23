using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCSEvent.API.Commons.DTO
{
    public class GuestSpeakerDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string Topic { get; set; }
        public int EventId { get; set; }
        public IFormFile Image { get; set; }

        public bool IsValid(out ValidationResponse source)
        {
            var response = new ValidationResponse();

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                string message = $"FirstName {ResponseCodes.DATA_IS_REQUIRED}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                string message = $"LastName {ResponseCodes.DATA_IS_REQUIRED}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            if (string.IsNullOrWhiteSpace(Biography))
            {
                string message = $"Biography {ResponseCodes.DATA_IS_REQUIRED}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            if (string.IsNullOrWhiteSpace(Topic))
            {
                string message = $"Topic {ResponseCodes.DATA_IS_REQUIRED}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            if (EventId <= 0)
            {
                string message = $"EventId {ResponseCodes.DATA_IS_REQUIRED}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            source = response;
            return true;
        }
    }

    public class UpdateGuestSpeakerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string Topic { get; set; }
        public int EventId { get; set; }
        public IFormFile Image { get; set; }
    }
}
