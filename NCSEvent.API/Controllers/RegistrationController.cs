using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("check-membership")]
        public async Task<IActionResult> IsMemberRegistered([FromBody] VerifyMembershipNoDTO request)
        {
            var response = await _registrationService.IsMemberRegistered(request);

            if (response.IsSuccessful)
            {
            return Ok(response);
        }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] FormDTO request)
        {
            var response = await _registrationService.Register(request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-all-guests")]
        public async Task<IActionResult> GetAllGuests(int EventId)
        {
            var response = await _registrationService.GetAllGuests(EventId);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
