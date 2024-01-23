using System.ComponentModel.DataAnnotations.Schema;

namespace NCSEvent.API.Entities
{
    public class EventImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int EventsId { get; set; }
        [ForeignKey("EventsId")]
        public Events Events { get; set; }
    }
}
