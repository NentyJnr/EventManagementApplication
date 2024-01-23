using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IEventManagementService _eventService;

        public ReportController(IReportService reportService, IEventManagementService eventService)
        {
            _reportService = reportService;
            _eventService = eventService;
        }

        [HttpPost("GetReport")]
        public async Task<IActionResult> GetReport([FromBody] EventFilterCriteria filterCriteria)
        {
            var response = await _reportService.GetAllEventReports(filterCriteria);

            if (response.IsSuccessful)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }
    }
}
