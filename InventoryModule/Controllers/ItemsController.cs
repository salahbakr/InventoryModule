using InventoryModule.Dtos;
using InventoryModule.Interfaces;
using InventoryModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var result = await _itemService.GetAllAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("id={id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var result = await _itemService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateItemDto dto)
        {
            var result = await _itemService.CreateAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] CreateItemDto dto, int id)
        {
            var result = await _itemService.UpdateAsync(dto, id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var result = await _itemService.DeleteAsync(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

    }
}
