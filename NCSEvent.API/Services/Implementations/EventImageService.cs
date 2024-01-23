using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;
using System.ComponentModel.Design;



namespace NCSEvent.API.Services.Implementations

{
    public class EventImageService : IEventImageService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EventManagementService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly string _uploadUrl;
       

        public EventImageService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<EventManagementService> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _uploadUrl = _configuration["ApplicationSettings:UploadUrl"];
            
        }


        
        public async Task<ServerResponse<bool>> CreateImage(EventImageDTO model)
        {
            var response = new ServerResponse<bool>();

            try
            {
                EventImage eventImage = new()
                {

                    //ImageUrl = GetImageUrl(imagefilepath),
                    EventsId = model.EventsId
                };
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (model.UploadImage.Length > 0)
                {

                    var extension = "." + model.UploadImage.FileName.Split('.')[model.UploadImage.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\EventImages\\");
                    string imagefilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(imagefilepath, model.UploadImage, directoryPath);
                    eventImage.ImageUrl = $"{_uploadUrl}{filename}";
 
                    _dbContext.ImageManagements.Add(eventImage);
                    await _dbContext.SaveChangesAsync();
                    response.IsSuccessful = true;
                    response.Data = true;
                    response.SuccessMessage = "Image Upoaded successfully.";
                    return response;
                }
                else
                {
                    // invalid file or empty file, please upload a file
                    return response;
                }  
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.IsSuccessful = false;
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = $"An error occurred: {"Image not Uploaded"}"
                };
                return response;
            }
        }

        

        private async Task<string> SaveFile(IFormFile file)
        {
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filename;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<bool> SaveFile(string filepath, IFormFile formFile, string directoryPath)
        {
            try
            {

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                using (Stream stream = File.Create(filepath))
                {
                    await formFile.CopyToAsync(stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                // log error

                throw;
            }
        }

        public async Task<bool> DeleteImage(int id)
        {
            try
            {
                var eventImage = await _dbContext.ImageManagements.FindAsync(id);

                if (eventImage == null)
                {
                    return false; // Entity not found
                }

                // Delete files
                await DeleteFile(eventImage.ImageUrl);
                await DeleteFile(eventImage.ImageUrl);

                _dbContext.ImageManagements.Remove(eventImage);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return false;
            }
        }

        private async Task DeleteFile(string filePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filePath);

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle file-deletion exceptions
                throw;
            }
        }

        public async Task<ServerResponse<EventImage>> GetAllRecordById(int id)
        {
            var response = new ServerResponse<EventImage>();

            try
            {
                var record = await _dbContext.ImageManagements
                    .Where(e => e.Id == id)
                    .Select(e => new EventImage
                    {
                        Id = e.Id,
                        ImageUrl = e.ImageUrl,
                        Events = e.Events,
                    })
                    .FirstOrDefaultAsync();

                if (record == null)
                {
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "Information not found."
                    };

                    return response;
                }

                response.IsSuccessful = true;
                response.Data = record;
                response.SuccessMessage = "Record Retrieved Successfully.";

            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = $"An error occurred while retrieving the event: {ex.Message}"
                };
            }

            return response;
        }
    }
}
