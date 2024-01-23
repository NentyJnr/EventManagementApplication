namespace NCSEvent.API.Entities
{
    public class HotelManagement : BaseObjectEntity
    {
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelType { get; set; }
        public string Contact { get; set; }
        public decimal Amount { get; set; }
        public string RoomType { get; set; }
        public DateTime RoomAvailability { get; set; }
    }

    public enum RoomType
    {
       All, Standard,  Double 
        
    }

}
