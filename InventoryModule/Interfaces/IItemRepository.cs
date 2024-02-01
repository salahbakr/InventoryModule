using InventoryModule.Entities;

namespace InventoryModule.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsWithCategoryAndShelf();
        Task<Item> GetItemByIdWithCategoryAndShelf(int id);
        Task<IEnumerable<Item>> GetItemsByIds(List<int> ids);
    }
}
