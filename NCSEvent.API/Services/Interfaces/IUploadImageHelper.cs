namespace NCSEvent.API.Services.Interfaces
{
    public interface IUploadImageHelper
    {
        Task<string> UploadImage(IFormFile imageFile);
    }
}
