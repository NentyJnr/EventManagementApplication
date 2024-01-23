using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.DTO;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;

namespace NCSEvent.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipManagementController : ControllerBase
    {
        private readonly IMembershipManagementService _uploadMemberService;

        public MembershipManagementController(IMembershipManagementService uploadMemberService)
        {
            _uploadMemberService = uploadMemberService;
        }


        [ProducesResponseType(typeof(ServerResponse<MembershipManagementDto>), StatusCodes.Status200OK)]
        [HttpPost("upload-member")]
        public async Task<IActionResult> UploadMember(MembershipManagementDto request)
        {
            var response = await _uploadMemberService.UploadMember(request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [ProducesResponseType(typeof(ServerResponse<MembershipManagementDto>), StatusCodes.Status200OK)]
        [HttpGet("get-all-uploadedMember")]
        public async Task<IActionResult> GetAllUploadMember()
        {
            var response = await _uploadMemberService.GetAllMembers();

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }
        [ProducesResponseType(typeof(ServerResponse<MembershipManagementDto>), StatusCodes.Status200OK)]
        [HttpGet("get-uploadedMember")]
        public async Task<IActionResult> GetHotelById([FromQuery] long memberId)
        {
            var response = await _uploadMemberService.GetMemberById(memberId);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return Ok(new ServerResponse<MembershipManagement> { Data = null, IsSuccessful = true, SuccessMessage = "member not found" });
            }
        }

        [ProducesResponseType(typeof(ServerResponse<HotelReturnDto>), StatusCodes.Status200OK)]
        [HttpPost("edit-uploadedMember")]
        public async Task<IActionResult> UpdateHotel(long memberlId, MembershipManagementDto request)
        {
            var response = await _uploadMemberService.UpdateMember(memberlId, request);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [ProducesResponseType(typeof(ServerResponse<HotelReturnDto>), StatusCodes.Status200OK)]
        [HttpDelete("delete-uploaded")]
        public async Task<IActionResult> DeleteHotel(long memberId)
        {
            var response = await _uploadMemberService.DeleteMember(memberId);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }

        [ProducesResponseType(typeof(ServerResponse<HotelReturnDto>), StatusCodes.Status200OK)]
        [HttpPost("activate-upload-member")]
        public async Task<IActionResult> ActivateUploadMember(long memberId)
        {
            var response = await _uploadMemberService.ActivateUploadMember(memberId);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [ProducesResponseType(typeof(ServerResponse<HotelReturnDto>), StatusCodes.Status200OK)]
        [HttpPost("deactivate-upload-member")]
        public async Task<IActionResult> DeactivateUploadMember(long memberId)
        {
            var response = await _uploadMemberService.DeactivateUploadMember(memberId);

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
