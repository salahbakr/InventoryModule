using InventoryModule.Data;
using InventoryModule.Entities;
using InventoryModule.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Repository
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Request>> GetAllAsync()
        {
            return await _context.Requests
                .Include(req => req.RequestItems)
                .ThenInclude(reqItem => reqItem.Item)
                .ToListAsync();
        }

        public override async Task<Request> GetByIdAsync(int id)
        {
            return await _context.Requests
                .Include(req => req.RequestItems)
                .ThenInclude(itemReq => itemReq.Item)
                .FirstAsync(req => req.Id == id);
        }
    }
}
