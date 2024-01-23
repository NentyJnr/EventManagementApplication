using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedback _feedback;
        public FeedbackController(IFeedback feedback)
        {
            _feedback = feedback;
        }

        [HttpPost("add-feedback")]
        public async Task<IActionResult> Create(FeedbackDTO request)
        {
            var response = await _feedback.Create(request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-all-feedbacks")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _feedback.GetAllRecord();

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }
    }
}
