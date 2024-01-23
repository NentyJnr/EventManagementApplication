using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;
using System.IO;

namespace NCSEvent.API.Services.Implementations
{

    public class UploadManagementService : IUploadManagementService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EventManagementService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly string _uploadUrl;

        public UploadManagementService(AppDbContext dbContext, ILogger<EventManagementService> logger, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _uploadUrl = _configuration["ApplicationSettings:UploadUrl"];
        }

        public async Task<ServerResponse<Uploads>> CreateUpload(UploadManagementDTO model)
        {
            var response = new ServerResponse<Uploads>();

            try
            {
                var uploadManagement = new Uploads
                {
                    MembershipPortalName = model.MembershipPortalName,
                    DateCreated = model.DateCreated,
                    Note = model.Note
                };
                string webRootPath = _webHostEnvironment.ContentRootPath;
                if (model.UploadLogo.Length > 0)
                {
                    var extension = "." + model.UploadLogo.FileName.Split('.')[model.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Logo\\");
                    string logofilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(logofilepath, model.UploadLogo, directoryPath);
                    uploadManagement.LogoUrl = $"{_uploadUrl}AdminImages/Logo/{filename}";
                }

                if (model.UploadSignature.Length > 0)
                {
                    var extension = "." + model.UploadLogo.FileName.Split('.')[model.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Signature\\");
                    string signaturefilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(signaturefilepath, model.UploadSignature, directoryPath);
                    uploadManagement.SignatureUrl = $"{_uploadUrl}AdminImages/Signature/{filename}";
                }

                _dbContext.Uploads.Add(uploadManagement);
                await _dbContext.SaveChangesAsync();

                response.Data = uploadManagement;
                response.SuccessMessage = "Image succesfully uploaded";
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.IsSuccessful = false;
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = $"An error occurred: {ex.Message}"
                };

                return null;
            }

            return response;
        }

        public async Task<ServerResponse<Uploads>> UpdateUpload(UpdateUploadDTO model)
        {
            var response = new ServerResponse<Uploads>();

            try
            {
                var uploadUpdate = _dbContext.Uploads.FirstOrDefault(u => u.Id == model.Id);
                if (uploadUpdate == null)
                {
                    response.IsSuccessful = false;
                    response.Data = null;
                    response.Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                        ResponseDescription = "No Record Found"
                    };

                    return response;
                }

                string webRootPath = _webHostEnvironment.ContentRootPath;
                if (model.UploadLogo.Length > 0)
                {
                    var extension = "." + model.UploadLogo.FileName.Split('.')[model.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Logo\\");
                    string logofilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(logofilepath, model.UploadLogo, directoryPath);
                    uploadUpdate.LogoUrl = $"{_uploadUrl}AdminImages/Logo/{filename}";
                }

                if (model.UploadSignature.Length > 0)
                {
                    var extension = "." + model.UploadLogo.FileName.Split('.')[model.UploadLogo.FileName.Split('.').Length - 1];
                    string filename = DateTime.Now.Ticks.ToString() + extension;
                    string directoryPath = Path.Combine(webRootPath + "\\Uploads\\AdminImages\\Signature\\");
                    string signaturefilepath = Path.Combine(directoryPath, filename.Trim());
                    await SaveFile(signaturefilepath, model.UploadSignature, directoryPath);
                    uploadUpdate.SignatureUrl = $"{_uploadUrl}AdminImages/Signature/{filename}";
                }

                uploadUpdate.MembershipPortalName = model.MembershipPortalName;
                uploadUpdate.Note = model.Note;

                _dbContext.Uploads.Update(uploadUpdate);
                await _dbContext.SaveChangesAsync();

                response.Data = uploadUpdate;
                response.SuccessMessage = "Image succesfully Updated";
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.IsSuccessful = false;
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = $"An error occurred: {ex.Message}"
                };
            }
            return response;
        }


        public string GetImageUrl(string imagePath)
        {
            var swaggerBaseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var imageEndpoint = "/api/Upload/Files";

            var imageUrl = $"{swaggerBaseUrl}{imageEndpoint}/{imagePath}";

            return imageUrl;
        }


        public async Task<string> SaveFile(IFormFile file)
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

                using (Stream stream =  File.Create(filepath))
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

        public async Task<bool> DeleteUpload(int id)
        {
            try
            {
                var uploadManagement = await _dbContext.Uploads.FindAsync(id);

                if (uploadManagement == null)
                {
                    return false; // Entity not found
                }

                // Delete files
                await DeleteFile(uploadManagement.LogoUrl);
                await DeleteFile(uploadManagement.SignatureUrl);

                _dbContext.Uploads.Remove(uploadManagement);
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

        public async Task<ServerResponse<Uploads>> GetRecordById(int id)
        {
            var response = new ServerResponse<Uploads>();

            try
            {
                var record = await _dbContext.Uploads
                    .Where(e => e.Id == id)
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


        public async Task<ServerResponse<List<Uploads>>> GetAllRecord()
        {
            var response = new ServerResponse<List<Uploads>>();

            try
            {
                var data = await _dbContext.Uploads
                    .Select(u => new Uploads
                    {
                        Id = u.Id,
                        MembershipPortalName = u.MembershipPortalName,
                        DateCreated = u.DateCreated,
                        Note = u.Note,
                        SignatureUrl = u.SignatureUrl,
                        LogoUrl = u.LogoUrl,
                    })
                    .ToListAsync();

                response.Data = data;
                response.IsSuccessful = true;
                response.SuccessMessage = "Record Retrieved Successfully.";
            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                    ResponseDescription = "Failed to fetch Uploads."
                };
                response.Data = new List<Uploads>();
            }

            return response;
        }
    }
}




