
using NCSEvent.API.Entities;

namespace NCSEvent.API.Commons.DTO
{
    public class EventManagementDTO
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IFormFile CoverImage { get; set; }
        public string Location { get; set; }
        public string Information { get; set; }
        public DateTime EventTime { get; set; }
      
    }

    public class EventUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IFormFile CoverImage { get; set; }
        public string Location { get; set; }
        public string Information { get; set; }
        public DateTime EventTime { get; set; }
    }

   public class EventModelView
   {
        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string Information { get; set; }
        public DateTime EventTime { get; set; }

    }
}
