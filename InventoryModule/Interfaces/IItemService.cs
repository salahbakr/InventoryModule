using InventoryModule.Dtos;

namespace InventoryModule.Interfaces
{
    public interface IItemService
    {
        public Task<ResponseModel<IEnumerable<ItemResponseDto>>> GetAllAsync();
        public Task<ResponseModel<ItemResponseDto>> GetByIdAsync(int id);
        public Task<ResponseModel<ItemResponseDto>> CreateAsync(CreateItemDto categoryDto);
        public Task<ResponseModel<ItemResponseDto>> UpdateAsync(CreateItemDto categoryDto, int id);
        public Task<ResponseModel<ItemResponseDto>> DeleteAsync(int id);
    }
}
