namespace NCSEvent.API.DTO
{
    public class MembershipManagementDto 
    {
        public string MemberShipCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
        public string MemberShipType { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
