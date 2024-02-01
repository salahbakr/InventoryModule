using InventoryModule.Dtos;
using InventoryModule.Entities;

namespace InventoryModule.Interfaces
{
    public interface IItemService
    {
        public Task<ResponseModel<IEnumerable<ItemResponseDto>>> GetAllAsync();
        public Task<ResponseModel<ItemResponseDto>> GetByIdAsync(int id);
        public Task<ResponseModel<ItemResponseDto>> CreateAsync(CreateItemDto categoryDto);
        public Task<ResponseModel<ItemResponseDto>> UpdateAsync(CreateItemDto categoryDto, int id);
        public Task<ResponseModel<ItemResponseDto>> DeleteAsync(int id);
        IEnumerable<Item> ChangeItemsQuantities(IEnumerable<Item> items, List<CreateRequestItemsDto> requestedItems, string operation);
    }
}
