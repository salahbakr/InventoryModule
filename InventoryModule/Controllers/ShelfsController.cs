using InventoryModule.Dtos;
using InventoryModule.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelfsController : ControllerBase
    {
        private readonly IShelfService _shelfService;

        public ShelfsController(IShelfService shelfService)
        {
            _shelfService = shelfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShelfsAsync()
        {
            var result = await _shelfService.GetAllAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetShelfByIdAsync(int id)
        {
            var result = await _shelfService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShelfAsync([FromBody] CreateShelfDto dto)
        {
            var result = await _shelfService.CreateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateShelfAsync([FromBody] CreateShelfDto dto, int id)
        {
            var result = await _shelfService.UpdateAsync(dto, id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteShelfAsync(int id)
        {
            var result = await _shelfService.DeleteAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
