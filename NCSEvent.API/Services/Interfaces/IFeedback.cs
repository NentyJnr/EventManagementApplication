using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IFeedback
    {
        Task<ServerResponse<Feedbacks>> Create(FeedbackDTO request);
        Task<ServerResponse<List<Feedbacks>>> GetAllRecord();
    }
}
