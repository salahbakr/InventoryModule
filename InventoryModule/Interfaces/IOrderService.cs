using InventoryModule.Dtos;
using InventoryModule.Entities;

namespace InventoryModule.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseModel<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync();

        Task CreateOrdersAsync(IEnumerable<Item> items);
    }
}