using InventoryModule.Entities;

namespace InventoryModule.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsWithCategoryAndShelf();
    }
}
