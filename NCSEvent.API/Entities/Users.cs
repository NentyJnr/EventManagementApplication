using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NCSEvent.API.Entities
{
    public class Users : IdentityUser
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
