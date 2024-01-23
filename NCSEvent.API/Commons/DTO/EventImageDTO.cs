namespace NCSEvent.API.Commons.DTO
{
    public class EventImageDTO
    {
        //public int Id { get; set; }
        public IFormFile UploadImage { get; set; }
        public int EventsId { get; set; }
    }

    public class UploadImageDTO
    {
        public IFormFile File { get; set; }
    }
}
