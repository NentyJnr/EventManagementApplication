using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Commons.DTO
{
    public class SessionDTO
    {
        public SessionDTO() { DateCreated = DateTime.UtcNow; }
        public string Token { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }

        public bool IsValid(out ValidationResponse source)
        {
            var response = new ValidationResponse();
            if (string.IsNullOrEmpty(Token))
            {
                var message = $"Token {ResponseCodes.DATA_IS_REQUIRED}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrEmpty(UserId))
            {
                var message = $"User {ResponseCodes.DATA_IS_REQUIRED}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            source = new ValidationResponse();
            return true;
        }
    }

    public class UpdateSessionDTO
    {
        public string Token { get; set; }
        public string UserId { get; set; }

        public bool IsValid(out ValidationResponse source)
        {
            var response = new ValidationResponse();
            if (string.IsNullOrEmpty(Token))
            {
                var message = $"Token {ResponseCodes.DATA_IS_REQUIRED}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            if (string.IsNullOrEmpty(UserId))
            {
                var message = $"User {ResponseCodes.DATA_IS_REQUIRED}";
                response.Code = ResponseCodes.DATA_IS_REQUIRED;
                response.Message = message;
                source = response;
                return false;
            }
            source = new ValidationResponse();
            return true;
        }

    }
}
