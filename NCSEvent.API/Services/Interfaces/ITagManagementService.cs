using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Services.Interfaces
{
    public interface ITagManagementService
    {
        Task<ServerResponse<TagModelResponse>> GenerateTag(TagDto request);
    }
}
