using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Commons.Extensions
{
    public class UploadImageHelper : IUploadImageHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly string _uploadUrl;

        public UploadImageHelper(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _uploadUrl = _configuration["ApplicationSettings:UploadUrl"];
        }
        public async Task<string> UploadImage(IFormFile imageFile)
        {
            string webRootPath = _webHostEnvironment.ContentRootPath;

            if (imageFile.Length > 0)
            {
                var extension = "." + imageFile.FileName.Split('.')[imageFile.FileName.Split('.').Length - 1];
                string imageFileName = $"{Guid.NewGuid()}{extension}";
                string imageDirectory = Path.Combine(webRootPath + "\\Uploads\\PassportUpload");
                string imageFilePath = Path.Combine(imageDirectory, imageFileName);
                var imageUrl = $"{_uploadUrl}PassportUpload/{imageFileName}";

                // Ensure the directory exists
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                // Save the file
                using (var stream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return imageUrl;
            }
            else
            {
                // Handle the case where the passportFile is empty
                throw new ArgumentException("Passport file is empty");
            }
        }
    }
}
