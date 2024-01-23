using NCSEvent.API.Entities;

namespace NCSEvent.API.Commons.DTO
{
    public class HotelManagementDto 
    {
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelType { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string Contact { get; set; }
        public decimal Amount { get; set; }
        public string RoomType { get; set; }
        public DateTime RoomAvailability { get; set; }
    }



    public class HotelReturnDto
    {
        public long? Id { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelType { get; set; }
        public string Contact { get; set; }
        public double Amount { get; set; }
        public string RoomType { get; set; }
        public DateTime RoomAvailability { get; set; }
    
    }
}
