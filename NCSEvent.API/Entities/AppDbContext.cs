using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.Models;

namespace NCSEvent.API.Entities
{
    public partial class AppDbContext : IdentityDbContext<Users, Roles, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Uploads> Uploads { get; set; }
        public virtual DbSet<MembershipType> MembershipTypes { get; set; }
        public virtual DbSet<HotelManagement> Hotels { get; set; }
        public virtual DbSet<MembershipManagement> UploadMembers { get; set; }
        public virtual DbSet<EventImage> ImageManagements { get; set; }
        public virtual DbSet<RegistrationForm> RegistrationForms { get; set; }
        public virtual DbSet<TransactionModel> Transactions { get; set; }
        public virtual DbSet<Sessions> Sessions { get; set; }
        public virtual DbSet<GuestSpeaker> GuestSpeakers { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<MessagingSystem>  MessagingSystems { get; set; }
    }

}
