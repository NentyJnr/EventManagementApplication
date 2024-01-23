using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Services.Interfaces
{
    public interface ISessionService
    {
        Task<ServerResponse<bool>> CreateSessionAsync(SessionDTO request);
        Task<ServerResponse<bool>> DeleteSessionAsync(string userId);
        //Task<ServerResponse<bool>> UpdateSessionAsync(UpdateSessionDTO request, string language);
    }
}
