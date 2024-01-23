namespace NCSEvent.API.Commons.DTO
{
    public class UploadManagementDTO
    {
        public string MembershipPortalName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Note { get; set; }
        public IFormFile UploadLogo { get; set; }
        public IFormFile UploadSignature { get; set; }

    }

    public class UpdateUploadDTO
    {
        public int Id { get; set; }
        public string MembershipPortalName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Note { get; set; }
        public IFormFile UploadLogo { get; set; }
        public IFormFile UploadSignature { get; set; }

    }

    public class UploadDTO
    {
        public IFormFile File { get; set; }
    }

}
