using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Entities
{
    public class EventFilterCriteria
    {
        public string? EventName { get; set; }
        public string? EventType { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? MembershipType { get; set; }

        public bool IsValid(out ValidationResponse source, string language)
        {
            var response = new ValidationResponse();

            // Check if at least one parameter is provided for filtering
            if (string.IsNullOrWhiteSpace(EventName)
                && string.IsNullOrWhiteSpace(EventType)
                && string.IsNullOrWhiteSpace(StartDate)
                && string.IsNullOrWhiteSpace(EndDate)
                && string.IsNullOrWhiteSpace(MembershipType))

            {
                string message = $"At least one parameter is required for filtering.";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            // Validate individual parameters if provided
            if (!string.IsNullOrWhiteSpace(EventName) && string.IsNullOrWhiteSpace(EventType))
            {
                string message = $" {(ResponseCodes.DATA_IS_REQUIRED, language)}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            // Add similar checks for other parameters as needed...

            source = response;
            return true;
        }
    }
}
