using AutoMapper;
using InventoryModule.Data;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRespository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRespository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ResponseModel<IEnumerable<CategoryResponseDto>>> GetAllAsync()
        {
            var categories = await _categoryRespository.GetAllAsync();

            if (categories.Count() == 0)
            {
                return new ResponseModel<IEnumerable<CategoryResponseDto>> 
                { 
                    Message = "There are no categories",
                    Error = "Error while retrieving categories" 
                };
            }

            var categoriesDto = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);

            return new ResponseModel<IEnumerable<CategoryResponseDto>> { Message = "Retrieved categories", Data = categoriesDto };
        }

        public async Task<ResponseModel<CategoryResponseDto>> GetByIdAsync(int id)
        {
            var category = await _categoryRespository.GetByIdAsync(id);

            if (category is null)
            {
                return NotFoundCategoryError();
            }

            return new ResponseModel<CategoryResponseDto>
            {
                Message = "Retrieved category",
                Data = _mapper.Map<CategoryResponseDto>(category)
            };
        }

        public async Task<ResponseModel<CategoryResponseDto>> CreateAsync(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            await _categoryRespository.CreateAsync(category);

            return new ResponseModel<CategoryResponseDto>
            {
                Message = "Successfully added a new category",
                Data = _mapper.Map<CategoryResponseDto>(category)
            };
        }

        public async Task<ResponseModel<CategoryResponseDto>> UpdateAsync(CreateCategoryDto categoryDto, int id)
        {
            var category = await _categoryRespository.GetByIdAsync(id);

            if (category is null)
            {
                return NotFoundCategoryError();
            }

            _mapper.Map(categoryDto, category);

            await _categoryRespository.UpdateAsync(category);

            return new ResponseModel<CategoryResponseDto>
            {
                Message = "Successfully updated category",
                Data = _mapper.Map<CategoryResponseDto>(category)
            };
        }

        public async Task<ResponseModel<CategoryResponseDto>> DeleteAsync(int id)
        {
            var category = await _categoryRespository.GetByIdAsync(id);

            if(category is null)
            {
                return NotFoundCategoryError();
            }

            await _categoryRespository.DeleteAsync(category);

            return new ResponseModel<CategoryResponseDto> 
            {
                Message = "Successfully removed category",
                Data = _mapper.Map<CategoryResponseDto>(category)
            };
        }

        internal ResponseModel<CategoryResponseDto> NotFoundCategoryError()
        {
            return new ResponseModel<CategoryResponseDto>
            {
                Message = "Can not find a category with that id",
                Error = "Error while retrieving category"
            };
        }
    }
}
