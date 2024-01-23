using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EventManagementController : ControllerBase
    {
        private readonly IEventManagementService _eventService;

        public EventManagementController(IEventManagementService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("create-event")]
        public async Task<IActionResult> Create([FromForm] EventManagementDTO eventDTO)
        {
            var response = await _eventService.Create(eventDTO);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [HttpPost("update-event")]
        public async Task<IActionResult> Update([FromForm] EventUpdateDTO eventDTO)
        {
            var response = await _eventService.Update(eventDTO);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpPost("activate-event/{id}")]
        public async Task<IActionResult> Activate(int id)
        {
            var response = await _eventService.Activate(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpPost("deactivate-event/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var response = await _eventService.Deactivate(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [HttpDelete("delete-event/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _eventService.Delete(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-all-events")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _eventService.GetAllRecord();

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-events/{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var response = await _eventService.GetRecordById(id);

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