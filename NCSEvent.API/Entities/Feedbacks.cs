using System.ComponentModel.DataAnnotations.Schema;

namespace NCSEvent.API.Entities
{
    public class Feedbacks : BaseEntity
    {
        public int Id { get; set; }
        public string Feedback { get; set; }
        public int Rating { get; set; }
        public string? Suggestion { get; set; }
        public int EventId { get; set; }
        [ForeignKey("EventId")]
        public Events Event { get; set; }
    }
}
