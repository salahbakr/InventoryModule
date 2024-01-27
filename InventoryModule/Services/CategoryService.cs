using AutoMapper;
using InventoryModule.Data;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Services
{
    /// <summary>
    /// Service class for managing categories.
    /// </summary>

    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRespository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The repository for category operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>

        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRespository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all categories asynchronously.
        /// </summary>
        /// <returns>A response model containing the list of categories.</returns>

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

        /// <summary>
        /// Gets a category by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>A response model containing the retrieved category.</returns>

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

        /// <summary>
        /// Creates a new category asynchronously.
        /// </summary>
        /// <param name="categoryDto">The data transfer object for creating a category.</param>
        /// <returns>A response model containing the created category.</returns>

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

        /// <summary>
        /// Updates an existing category asynchronously.
        /// </summary>
        /// <param name="categoryDto">The data transfer object for updating a category.</param>
        /// <param name="id">The ID of the category to update.</param>
        /// <returns>A response model containing the updated category.</returns>

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

        /// <summary>
        /// Deletes a category by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A response model containing the deleted category.</returns>

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

        /// <summary>
        /// Handles the case when a category is not found.
        /// </summary>
        /// <returns>A response model indicating a category not found error.</returns>

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
