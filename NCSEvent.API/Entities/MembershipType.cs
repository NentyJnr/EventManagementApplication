using System.ComponentModel.DataAnnotations.Schema;

namespace NCSEvent.API.Entities
{
    public class MembershipType : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Events Event { get; set; }
    }
}
