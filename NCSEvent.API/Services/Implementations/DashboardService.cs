using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _dbContext;

        public DashboardService(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ServerResponse<DashboardDTO>> GetDashboardKPIsAsync()
        {
            var response = new ServerResponse<DashboardDTO>();

            try
            {
                var totalEvents = await _dbContext.Events.CountAsync();
                var totalUpcomingEvents = await _dbContext.Events.CountAsync(e => e.StartDate > DateTime.Now);
                var totalOngoingEvents = await _dbContext.Events.CountAsync(e => e.StartDate.Date <= DateTime.Now.Date && e.EndDate.Date >= DateTime.Now.Date);
                var totalPastEvents = await _dbContext.Events.CountAsync(e => e.EndDate.Date < DateTime.Now.Date);


                var data = new DashboardDTO
                {
                    TotalEvents = totalEvents,
                    TotalUpcomingEvents = totalUpcomingEvents,
                    TotalOngoingEvents = totalOngoingEvents,
                    TotalPastEvents = totalPastEvents
                };

                response.IsSuccessful = true;
                response.Data = data;
            }
            catch
            {
                response.IsSuccessful = false;
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = "Unsuccessful"
                };
            }

            return response;
        }
    }
}
