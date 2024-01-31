using InventoryModule.Entities;

namespace InventoryModule.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task CreateAsync(List<Order> orders);
    }
}
