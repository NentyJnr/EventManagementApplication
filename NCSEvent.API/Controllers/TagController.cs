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
    public class TagController : ControllerBase
    {
   
        private readonly ITagManagementService _tagManagement;

        public TagController(ITagManagementService tagManagement)
        {         
            _tagManagement = tagManagement;
        }

        [ProducesResponseType(typeof(ServerResponse<TagModelResponse>), StatusCodes.Status200OK)]
        [HttpPost("generate-tag")]
        public async Task<IActionResult> GenerateTag(TagDto request)
        {
            var result = await _tagManagement.GenerateTag(request);

            if (result.IsSuccessful)
            {
                return Ok(result);
            }

            return Unauthorized(result.Error);
        }
    }
}
