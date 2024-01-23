using NCSEvent.API.Entities;

namespace NCSEvent.API.Commons.DTO
{
    public class LoginResponse
    {
        public UserDTO UserDTO { get; set; }
        public IList<string> Roles { get; set; }
        public string token { get; set; }
    }
}
