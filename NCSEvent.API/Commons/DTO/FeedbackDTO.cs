using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Commons.DTO
{
    public class FeedbackDTO
    {
        public string Feedback { get; set; }
        public int Rating { get; set; }
        public string? Suggestion { get; set; }
        public int EventId { get; set; }

        public bool IsValid(out ValidationResponse source)
        {
            var response = new ValidationResponse();

            if (string.IsNullOrWhiteSpace(Feedback))
            {
                string message = $"Feedback {ResponseCodes.DATA_IS_REQUIRED}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            if (Rating <= 0 || Rating > 5)
            {
                string message = $"Rating {ResponseCodes.DATA_IS_REQUIRED}";
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
}
