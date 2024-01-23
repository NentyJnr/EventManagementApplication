using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IGuestSpeaker
    {
        Task<ServerResponse<GuestSpeaker>> Create(GuestSpeakerDTO request);
        Task<ServerResponse<GuestSpeaker>> Update(UpdateGuestSpeakerDTO request);
        Task<ServerResponse<bool>> Activate(int id);
        Task<ServerResponse<bool>> Deactivate(int id);
        Task<ServerResponse<bool>> Delete(int id);
        Task<ServerResponse<List<GuestSpeaker>>> GetAllRecord();
        Task<ServerResponse<GuestSpeaker>> GetRecordById(int id);
    }
}
