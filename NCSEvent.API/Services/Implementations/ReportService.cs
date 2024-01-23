using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;
using static NCSEvent.API.Commons.DTO.ReportModelView;

namespace NCSEvent.API.Services.Implementations
{
    public class ReportService : IReportService
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<AccountService> _logger;
        private readonly IEventManagementService _eventManagementService;
        private readonly IMembershipTypeService _membershipTypeService;

        public ReportService(AppDbContext dbContext, ILogger<AccountService> logger, IEventManagementService eventManagementService, IMembershipTypeService membershipTypeService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventManagementService = eventManagementService;
            _membershipTypeService = membershipTypeService;
        }

        public async Task<ServerResponse<List<ReportModelView>>> FilterEvents(EventFilterCriteria criteria)
        {
            try
            {
                var filteredEvents = await _dbContext.Events
                    .Where(e =>
                        (string.IsNullOrWhiteSpace(criteria.EventName) || e.Name.Contains(criteria.EventName)) &&
                        (string.IsNullOrWhiteSpace(criteria.StartDate) || e.StartDate >= Convert.ToDateTime(criteria.StartDate)) &&
                        (string.IsNullOrWhiteSpace(criteria.EndDate) || e.EndDate <= Convert.ToDateTime(criteria.EndDate)) &&
                        (string.IsNullOrWhiteSpace(criteria.EventType) || e.EventType.Contains(criteria.EventType)) &&
                        (string.IsNullOrWhiteSpace(criteria.MembershipType) || e.MembershipTypes.Any(mt => mt.Name == criteria.MembershipType))

                    )
                    .ToListAsync();

                var reportModelView = filteredEvents.Select(e => new ReportModelView
                {
                    EventName = e.Name,
                    EventType = e.EventType,
                    EndDate = e.EndDate,
                    StartDate = e.StartDate,
                    
                    MembershipTypes = e.MembershipTypes,
                       
                    
                }).ToList();

                return new ServerResponse<List<ReportModelView>>
                {
                    IsSuccessful = true,
                    //Data = reportModelViews
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<List<ReportModelView>>
                {
                    IsSuccessful = false,
                    Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                        ResponseDescription = $"An error occurred while filtering events: {ex.Message}"
                    }
                };
            }
        }

        public async Task<ServerResponse<List<ReportModelView>>> GetAllEventReports(EventFilterCriteria filterCriteria)
        {
            try
            {

                var allEventsResponse = await _eventManagementService.GetAllRecord(); // Adjust the method name based on your actual service
                if (!allEventsResponse.IsSuccessful)
                {
                    return new ServerResponse<List<ReportModelView>>
                    {
                        IsSuccessful = false,
                        Error = new ErrorResponse
                        {
                            ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                            ResponseDescription = $"An error occurred while retrieving all events: {allEventsResponse.Error?.ResponseDescription}"
                        }
                    };
                }


                var allEvents = allEventsResponse.Data;

                var reportData = allEvents.Select(e => new ReportModelView
                {
                    // Map properties from Event entity to ReportModelView
                    EventName = e.Name,
                    EventType = e.EventType,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    MembershipTypes = e.MembershipTypes,

                }).ToList();

                return new ServerResponse<List<ReportModelView>>
                {
                    IsSuccessful = true,
                    Data = reportData
                };
            }
            catch (Exception ex)
            {
                return new ServerResponse<List<ReportModelView>>
                {
                    IsSuccessful = false,
                    Error = new ErrorResponse
                    {
                        ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                        ResponseDescription = $"An error occurred while retrieving event reports: {ex.Message}"
                    }
                };
            }
        }

    }

}
