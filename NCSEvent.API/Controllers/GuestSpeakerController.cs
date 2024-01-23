using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestSpeakerController : ControllerBase
    {
        private readonly IGuestSpeaker _guestSpeaker;
        public GuestSpeakerController(IGuestSpeaker guestSpeaker)
        {
            _guestSpeaker = guestSpeaker;
        }

        [HttpPost("create-speaker")]
        public async Task<IActionResult> Create([FromForm] GuestSpeakerDTO request)
        {
            var response = await _guestSpeaker.Create(request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [HttpPost("update-speaker")]
        public async Task<IActionResult> Update([FromForm] UpdateGuestSpeakerDTO request)
        {
            var response = await _guestSpeaker.Update(request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpPost("activate-speaker/{id}")]
        public async Task<IActionResult> Activate(int id)
        {
            var response = await _guestSpeaker.Activate(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpPost("deactivate-speaker/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var response = await _guestSpeaker.Deactivate(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [HttpDelete("delete-speaker/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _guestSpeaker.Delete(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-all-speakers")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _guestSpeaker.GetAllRecord();

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-speaker/{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var response = await _guestSpeaker.GetRecordById(id);

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
