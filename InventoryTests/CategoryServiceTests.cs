using AutoMapper;
using InventoryModule.Data;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Interfaces;
using InventoryModule.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTests
{
    public class CategoryServiceTests
    {
        private readonly ICategoryService _categoryService;
        private readonly Mock<IGenericRepository<Category>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        public CategoryServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IGenericRepository<Category>>();
            _categoryService = new CategoryService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCategoriesWithMessage()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };

            var expectedCategoriesResponse = new List<CategoryResponseDto>
            {
                new CategoryResponseDto { Id = 1, Name = "Category 1" },
                new CategoryResponseDto { Id = 2, Name = "Category 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CategoryResponseDto>>(categories)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Retrieved categories", result.Message);
            Assert.Equal(2, result.Data.Count());
            Assert.Equal("Category 1", result.Data.First().Name);
            Assert.Equal("Category 2", result.Data.Last().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsNotFound_WhenNoCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>();

            var expectedCategoriesResponse = new List<CategoryResponseDto>();

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CategoryResponseDto>>(categories)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenCategoryNotFound()
        {
            // Arrange
            var categoryId = 1;
            Category category = null;
            CategoryResponseDto expectedCategoriesResponse = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryResponseDto>(category)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenCategoryFound()
        {
            // Arrange
            var categoryId = 1;
            Category category = new Category { Id = categoryId, Name = "Category1"};
            CategoryResponseDto expectedCategoriesResponse = new CategoryResponseDto { Id = categoryId, Name = "Category1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryResponseDto>(category)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #region

        [Fact]
        public async Task CreateAsync_ShouldAddNewCategory()
        {
            // Arrange
            CreateCategoryDto createCategoryDto = new CreateCategoryDto { Name = "Category1" };
            Category category = new Category { Id = 1, Name = "Category1" };
            CategoryResponseDto expectedCategoriesResponse = new CategoryResponseDto { Id = 1, Name = "Category1" };

            _mockMapper.Setup(mapper => mapper.Map<Category>(createCategoryDto)).Returns(category);
            _mockRepository.Setup(repo => repo.CreateAsync(category)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryResponseDto>(category)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.CreateAsync(createCategoryDto);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion

        [Fact]
        public async Task UpdateAsync_ReturnNotFound_WhenNoCategoryFound()
        {
            // Arrange
            var categoryId = 1;
            CreateCategoryDto createCategoryDto = new CreateCategoryDto { Name = "Category1" };
            Category category = null;
            CategoryResponseDto expectedCategoriesResponse = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map(createCategoryDto, category)).Returns(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryResponseDto>(category)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.UpdateAsync(createCategoryDto, categoryId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task UpdateAsync_UpdateCategory_WhenCategoryFound()
        {
            // Arrange
            var categoryId = 1;
            CreateCategoryDto createCategoryDto = new CreateCategoryDto { Name = "Category1" };
            Category category = new Category { Id = categoryId, Name = "Category1" };
            CategoryResponseDto expectedCategoriesResponse = new CategoryResponseDto { Id = categoryId, Name = "Category1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map(createCategoryDto, category)).Returns(category);
            _mockRepository.Setup(repo => repo.UpdateAsync(category)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryResponseDto>(category)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.UpdateAsync(createCategoryDto, categoryId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }


        [Fact]
        public async Task DeleteAsync_ReturnNotFound_WhenNoCategoryFound()
        {
            // Arrange
            var categoryId = 1;
            CreateCategoryDto createCategoryDto = new CreateCategoryDto { Name = "Category1" };
            Category category = null;
            CategoryResponseDto expectedCategoriesResponse = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockRepository.Setup(repo => repo.DeleteAsync(category)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryResponseDto>(category)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.UpdateAsync(createCategoryDto, categoryId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCategory_WhenCategoryFound()
        {
            // Arrange
            var categoryId = 1;
            CreateCategoryDto createCategoryDto = new CreateCategoryDto { Name = "Category1" };
            Category category = new Category { Id = 1, Name = "Category1" };
            CategoryResponseDto expectedCategoriesResponse = new CategoryResponseDto { Id = 1, Name = "Category1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockRepository.Setup(repo => repo.DeleteAsync(category)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryResponseDto>(category)).Returns(expectedCategoriesResponse);

            // Act
            var result = await _categoryService.UpdateAsync(createCategoryDto, categoryId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }
    }
}