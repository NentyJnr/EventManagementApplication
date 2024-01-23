namespace NCSEvent.API.Entities
{
    public class Uploads
    {
        public int Id { get; set; }
        public string MembershipPortalName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Note { get; set; }
        public string SignatureUrl { get; set; }
        public string LogoUrl { get; set; } 
    }
}
