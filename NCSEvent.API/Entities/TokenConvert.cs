using NCSEvent.API.Commons.DTO;

namespace NCSEvent.API.Entities
{
    public class TokenConvert
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserDTO User { get; set; }


    }
}
