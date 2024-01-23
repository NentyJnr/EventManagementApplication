using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Services.Implementations;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventImageController : ControllerBase
    {
        private readonly IEventImageService _eventImageService;

        public EventImageController(IEventImageService eventImageService)
        {
            _eventImageService = eventImageService;
        }

        [HttpPost]
        [Route("Upload-Image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImage([FromForm] EventImageDTO model)
        {
            var result = await _eventImageService.CreateImage(model);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to create image.");
            }
        }

        [HttpDelete]
        [Route("DeleteImage/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var result = await _eventImageService.DeleteImage(id);
            if (result)
            {
                return Ok("UploadManagement deleted successfully.");
            }
            else
            {
                return BadRequest("Failed to delete UploadManagement.");
            }
        }

        [HttpGet("get-image/{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var response = await _eventImageService.GetAllRecordById(id);

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

