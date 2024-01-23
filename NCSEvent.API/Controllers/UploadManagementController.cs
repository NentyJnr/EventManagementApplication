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
    public class UploadManagementController : ControllerBase
    {
        private readonly IUploadManagementService _uploadManagementService;

        public UploadManagementController(IUploadManagementService uploadManagementService)
        {
            _uploadManagementService = uploadManagementService;
        }

        [HttpPost]
        [Route("CreateUpload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUpload([FromForm] UploadManagementDTO model)
        {
            var result = await _uploadManagementService.CreateUpload(model);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to create upload.");
            }
        }

        [HttpPost]
        [Route("UpdateUpload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUpload([FromForm] UpdateUploadDTO model)
        {
            var result = await _uploadManagementService.UpdateUpload(model);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to Update.");
            }
        }

        [HttpDelete]
        [Route("DeleteUpload/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUpload(int id)
        {
            var result = await _uploadManagementService.DeleteUpload(id);
            if (result)
            {
                return Ok("Upload deleted successfully.");
            }
            else
            {
                return BadRequest("Failed to delete Upload.");
            }
        }

        [HttpGet("get-upload/{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            var response = await _uploadManagementService.GetRecordById(id);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Error);
            }
        }


        [HttpGet("get-all-uploads")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _uploadManagementService.GetAllRecord();

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
