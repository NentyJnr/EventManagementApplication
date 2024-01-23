using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IEventImageService
    {

        Task<ServerResponse<bool>> CreateImage(EventImageDTO model);
        Task<bool> DeleteImage(int id);
        Task<ServerResponse<EventImage>> GetAllRecordById(int id);
    }
}
