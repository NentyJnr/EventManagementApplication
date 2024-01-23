using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Commons.DTO
{
    public class MembershipTypeDTO : BaseObjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int EventId { get; set; }

        public bool IsValid(out ValidationResponse source)
        {
            var response = new ValidationResponse();

            if (string.IsNullOrWhiteSpace(Name))
            {
                string message = $"Name {ResponseCodes.DATA_IS_REQUIRED}";
                response.Message = message;
                response.Code = ResponseCodes.DATA_IS_REQUIRED;

                source = response;
                return false;
            }

            if (Amount < 0)
            {
                string message = $"Amount {ResponseCodes.DATA_IS_REQUIRED}";
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
