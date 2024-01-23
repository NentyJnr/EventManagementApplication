namespace NCSEvent.API.Entities
{
    public class BaseEntity
    {
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive   { get; set; }
        public bool IsDeactivated { get; set; }

    }

    public class BaseObjectEntity
    {
        public long Id { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
