using NCSEvent.API.Entities;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IEventManagementService
    {
        Task<ServerResponse<Events>> Create(EventManagementDTO events);
        Task<ServerResponse<Events>> Update(EventUpdateDTO events);
        Task<ServerResponse<bool>> Activate(int id);
        Task<ServerResponse<bool>> Deactivate(int id);
        Task<ServerResponse<bool>> Delete(int id);
        Task<ServerResponse<List<Events>>> GetAllRecord();
        Task<ServerResponse<Events>> GetRecordById(int id);
    }
}
