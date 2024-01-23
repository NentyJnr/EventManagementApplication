namespace NCSEvent.API.Entities
{
    public class ReportFilterParameters
    {
        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MembershipType { get; set; }
    }
}
