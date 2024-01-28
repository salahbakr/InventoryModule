

using InventoryModule.Entities;

namespace InventoryModule.Interfaces
{
    public interface IRequestRepository : IGenericRepository<Request>
    {
        Task<IEnumerable<Request>> GetAllAsync();
        Task<Request> GetByIdAsync(int id);
    }
}