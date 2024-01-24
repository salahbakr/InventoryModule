using InventoryModule.Dtos;

namespace InventoryModule.Interfaces
{
    public interface ICategoryService
    {
        public Task<ResponseModel<IEnumerable<CategoryResponseDto>>> GetAllAsync();
        public Task<ResponseModel<CategoryResponseDto>> GetByIdAsync(int id);
        public Task<ResponseModel<CategoryResponseDto>> CreateAsync(CreateCategoryDto categoryDto);
        public Task<ResponseModel<CategoryResponseDto>> UpdateAsync(CreateCategoryDto categoryDto, int id);
        public Task<ResponseModel<CategoryResponseDto>> DeleteAsync(int id);
    }
}