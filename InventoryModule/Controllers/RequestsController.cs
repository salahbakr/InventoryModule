using InventoryModule.Dtos;
using InventoryModule.Enums;
using InventoryModule.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {

        private readonly IRequestService _requestService;

        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequestsAsync()
        {
            var result = await _requestService.GetAllAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetRequestByIdAsync(int id)
        {
            var result = await _requestService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetRequestByIdAsync([FromBody] CreateRequestDto dto)
        {
            var result = await _requestService.CreateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("id={id}")]
        public async Task<IActionResult> ChangeRequestStatusAsync([FromForm] Status status, int id)
        {
            var result = await _requestService.ChangeRequestStatus(id, status);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
