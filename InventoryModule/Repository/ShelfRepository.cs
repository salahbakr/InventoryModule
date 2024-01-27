using InventoryModule.Data;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Repository
{
    public class ShelfRepository : GenericRepository<Shelf>, IShelfRepository
    {
        private readonly ApplicationDbContext _context;

        public ShelfRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shelf>> GetAllShelfsWithItems()
        {
            return await _context.Shelfs.Include(shelf => shelf.Items).ToListAsync();
        }
    }
}