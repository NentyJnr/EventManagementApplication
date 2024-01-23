using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IMembershipTypeService
    {
        Task<ServerResponse<MembershipType>> Create(MembershipTypeDTO events);
        Task<ServerResponse<MembershipType>> Update(MembershipTypeDTO events);
        Task<ServerResponse<bool>> Activate(int id);
        Task<ServerResponse<bool>> Deactivate(int id);
        Task<ServerResponse<bool>> Delete(int id);
        Task<ServerResponse<List<MembershipType>>> GetAllRecord();
        Task<ServerResponse<MembershipType>> GetAllRecordById(int id);
    }
}
