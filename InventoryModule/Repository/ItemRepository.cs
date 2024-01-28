using InventoryModule.Data;
using InventoryModule.Entities;
using InventoryModule.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsWithCategoryAndShelf()
        {
            return await _context.Items.Include(item => item.Category).Include(item => item.Shelf).ToListAsync();
        }
    }
}
