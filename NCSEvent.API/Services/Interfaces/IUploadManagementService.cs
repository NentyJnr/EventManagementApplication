using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IUploadManagementService
    {

        Task<ServerResponse<Uploads>> CreateUpload(UploadManagementDTO model);
        Task<ServerResponse<Uploads>> UpdateUpload(UpdateUploadDTO model);
        Task<bool> DeleteUpload(int id);
        Task<ServerResponse<List<Uploads>>> GetAllRecord();
        Task<ServerResponse<Uploads>> GetRecordById(int id);
    }
}




