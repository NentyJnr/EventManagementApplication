namespace NCSEvent.API.Entities
{
    public class MessagingSystem : BaseEntity
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public bool Status { get; set; }
    }
}
