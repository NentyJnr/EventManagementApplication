using NCSEvent.API.Commons.Responses;
using NCSEvent.API.DTO;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IMembershipManagementService
    {
        Task<ServerResponse<MembershipManagement>> UploadMember(MembershipManagementDto request);
        Task<ServerResponse<List<MembershipManagement>>> GetAllMembers();
        Task<ServerResponse<MembershipManagement>> GetMemberById(long memberId);
        Task<ServerResponse<bool>> UpdateMember(long memberId, MembershipManagementDto request);
        Task<ServerResponse<bool>> DeleteMember(long memberId);
        Task<ServerResponse<bool>> DeactivateUploadMember(long memberId);
        Task<ServerResponse<bool>> ActivateUploadMember(long memberId);
    }
}
