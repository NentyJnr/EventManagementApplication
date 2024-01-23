using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipTypeController : ControllerBase
    {
        private readonly IMembershipTypeService _membershipTypeService;

        public MembershipTypeController(IMembershipTypeService membershipTypeService)
        {
            _membershipTypeService = membershipTypeService;
        }

        [HttpPost("create-membershipType")]
        public async Task<IActionResult> Create(MembershipTypeDTO request)
        {
            var response = await _membershipTypeService.Create(request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [HttpPost("update-membershipType")]
        public async Task<IActionResult> Update(MembershipTypeDTO request)
        {
            var response = await _membershipTypeService.Update(request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpPost("activate/{id}")]
        public async Task<IActionResult> Activate(int id)
        {
            var response = await _membershipTypeService.Activate(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var response = await _membershipTypeService.Deactivate(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _membershipTypeService.Delete(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-all-membershipType")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _membershipTypeService.GetAllRecord();

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-membershipType/{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var response = await _membershipTypeService.GetAllRecordById(id);

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
