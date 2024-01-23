namespace NCSEvent.API.Commons.DTO
{
    public class BaseObjectDTO
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeactivated { get; set; }
    }
}
