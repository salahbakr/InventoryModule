using InventoryModule.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var result = await _orderService.GetAllOrdersAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
