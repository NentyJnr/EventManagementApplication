using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _accountService;
        public AccountController(IAccount accountService)
        {
            _accountService = accountService;
        }

        [ProducesResponseType(typeof(ServerResponse<UserDTO>), StatusCodes.Status200OK)]
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _accountService.GetAllRecord();

            if (response.IsSuccessful)
            {
                return Ok(response);
            }

            return BadRequest(response.Error);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(ServerResponse<LoginResponse>), StatusCodes.Status200OK)]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LoginRequest request)
        {
            var result = await _accountService.Login(request.Email, request.Password);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return Unauthorized(result.Error);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            var result = await _accountService.LogOut();

            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return Unauthorized(result.Error);
        }
    }
}
