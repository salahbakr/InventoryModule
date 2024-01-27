using InventoryModule.Dtos;

namespace InventoryModule.Interfaces
{
    public interface IShelfService
    {
        public Task<ResponseModel<IEnumerable<ShelfResponseDto>>> GetAllAsync();
        public Task<ResponseModel<ShelfResponseDto>> GetByIdAsync(int id);
        public Task<ResponseModel<ShelfResponseDto>> CreateAsync(CreateShelfDto shelfDto);
        public Task<ResponseModel<ShelfResponseDto>> UpdateAsync(CreateShelfDto shelfDto, int id);
        public Task<ResponseModel<ShelfResponseDto>> DeleteAsync(int id);
    }
}