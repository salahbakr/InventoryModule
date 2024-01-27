using InventoryModule.Dtos;
using InventoryModule.Entities;

namespace InventoryModule.Interfaces
{
    public interface IShelfRepository : IGenericRepository<Shelf>
    {
        Task<IEnumerable<Shelf>> GetAllShelfsWithItems();
    }
}
