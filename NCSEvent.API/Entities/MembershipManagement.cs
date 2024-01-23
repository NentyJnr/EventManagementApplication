namespace NCSEvent.API.Entities
{
    public class MembershipManagement : BaseObjectEntity
    {
        public string MemberShipCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
        public string MemberShipType { get; set; }
    }

}