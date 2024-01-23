using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using System.Reflection.Metadata;

namespace NCSEvent.API.Commons.DTO
{
    public class ReportModelView
    {
        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<MembershipType> MembershipTypes { get; set; }

    }
}