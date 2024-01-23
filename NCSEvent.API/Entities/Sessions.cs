namespace NCSEvent.API.Entities
{
    public class Sessions : BaseEntity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
