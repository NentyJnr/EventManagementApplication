namespace NCSEvent.API.Commons.Models
{
    public class EmailBodyRequest
    {
        public long GuestId { get; set; }
        public int EventId { get; set; }
        public string FeedbackLink { get; set; }
    }
}
