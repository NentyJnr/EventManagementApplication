using System.ComponentModel.DataAnnotations.Schema;

namespace NCSEvent.API.Entities
{
    public class GuestSpeaker : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string Topic { get; set; }
        public string ImageUrl { get; set; }
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Events Event { get; set; }
    }
}
