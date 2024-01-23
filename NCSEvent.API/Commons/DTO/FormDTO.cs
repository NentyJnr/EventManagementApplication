using NCSEvent.API.Entities;

namespace NCSEvent.API.Commons.DTO
{
    public class FormDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long? HotelId { get; set; }
        public IFormFile UploadPassport { get; set; }
        //public decimal Amount { get; set; }
        public bool IsMember = false;
        public int EventManagementId { get; set; }
    }
}
