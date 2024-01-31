using InventoryModule.Data;
using InventoryModule.Entities;
using InventoryModule.Interfaces;

namespace InventoryModule.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateAsync(List<Order> orders)
        {
            await _context.Set<Order>().AddRangeAsync(orders);
        }
    }
}
