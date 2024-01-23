using Microsoft.AspNetCore.Identity;

namespace NCSEvent.API.Entities
{
    public class Roles : IdentityRole
    {
        public string Description { get; set; }
    }
}
