using System.ComponentModel.DataAnnotations.Schema;

namespace NCSEvent.API.Entities
{
    public class RegistrationForm : BaseObjectEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportUrl { get; set; }
        public int EventManagementId { get; set; }
        [ForeignKey("EventManagementId")]
        public Events Events { get; set; }
        public bool PaymentConfirmed { get; set; } = false;
        public bool FeedbackEmailSent { get; set; } = false;
    }

}
