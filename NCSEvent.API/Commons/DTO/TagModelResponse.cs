using NCSEvent.API.Entities;

namespace NCSEvent.API.Commons.DTO
{
    public class TagModelResponse
    {
            public string uniqueId { get; set; }
            public string SignatureUrl { get; set; }
            public string LogoUrl { get; set; }
            public string FullName { get; set; }
            public string ProfilePictureUrl { get; set; }
            public DateTime EventStartDate { get; set; }
            public DateTime EventEndDate { get; set; }
            public string EventName { get; set; }            

    }
} 
