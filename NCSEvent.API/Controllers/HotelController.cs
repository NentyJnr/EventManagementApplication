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
    public class HotelController : ControllerBase
    {
        
        private readonly IHotelManagementService _hotelService;

        public HotelController(IHotelManagementService hotelService)
        {           
            _hotelService = hotelService;
        }

        [ProducesResponseType(typeof(ServerResponse<HotelReturnDto>), StatusCodes.Status200OK)]
        [HttpPost("create-hotel")]
        public async Task<IActionResult> CreateHotel(HotelManagementDto request)
        {
            var response = await _hotelService.CreateHotel(request);

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
        [HttpGet("get-all-hotel")]
        public async Task<IActionResult> GetAllHotel()
        {
            var response = await _hotelService.GetHotels();

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
        [HttpGet("get-hotel")]
        public async Task<IActionResult> GetHotelById([FromQuery]long hotelId)
        {
            var response = await _hotelService.GetHotelById(hotelId);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return Ok(new ServerResponse<HotelManagement> { Data = null, IsSuccessful = true, SuccessMessage = "hotel not found" });
            }
        }

        [ProducesResponseType(typeof(ServerResponse<HotelReturnDto>), StatusCodes.Status200OK)]
        [HttpPost("edit-hotel")]
        public async Task<IActionResult> UpdateHotel(long hotelId, HotelManagementDto request)
        {
            var response = await _hotelService.UpdateHotel(hotelId, request);

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
        [HttpDelete("delete-hotel")]
        public async Task<IActionResult> DeleteHotel(long hotelId)
        {
            var response = await _hotelService.DeleteHotel(hotelId);

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
        [HttpPost("activate-hotel")]
        public async Task<IActionResult> ActivateHotel(long hotelId)
        {
            var response = await _hotelService.ActivateHotel(hotelId);

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
        [HttpPost("deactivate-hotel")]
        public async Task<IActionResult> DeactivateHotel(long hotelId)
        {
            var response = await _hotelService.DeactivateHotel(hotelId);

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
