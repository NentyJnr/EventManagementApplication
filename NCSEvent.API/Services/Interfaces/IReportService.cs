using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;


namespace NCSEvent.API.Services.Interfaces
{
    public interface IReportService
    {
        Task<ServerResponse<List<ReportModelView>>> FilterEvents(EventFilterCriteria criteria);

        Task<ServerResponse<List<ReportModelView>>> GetAllEventReports(EventFilterCriteria filterCriteria);

    }
}
