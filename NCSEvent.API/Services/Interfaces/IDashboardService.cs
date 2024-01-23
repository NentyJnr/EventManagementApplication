using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;

namespace NCSEvent.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<ServerResponse<DashboardDTO>> GetDashboardKPIsAsync();
    }
}
