using System.ComponentModel.DataAnnotations.Schema;

namespace NCSEvent.API.Entities
{
    public class Events : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? CoverImage { get; set; }
        public ICollection<MembershipType> MembershipTypes { get; set; }
        public ICollection<EventImage> EventImages { get; set; }
        public ICollection<GuestSpeaker> GuestSpeakers { get; set; }
        public ICollection<Feedbacks> Feedbacks { get; set; }
        public string Location { get; set; }
        public string Information { get; set; }
        public DateTime? EventTime { get; set; }
    }

}

