using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Interfaces;
using InventoryModule.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryTests
{
    public class ItemServiceTests
    {
        /// <summary>
        /// Unit tests for the <see cref="ItemsService"/> class.
        /// </summary>

        private readonly IItemService _itemService;
        private readonly Mock<IItemRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IGenericRepository<Shelf>> _mockShelfRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemServiceTests"/> class.
        /// </summary>

        public ItemServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IItemRepository>();
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();
            _mockShelfRepository = new Mock<IGenericRepository<Shelf>>();
            _mockRepository = new Mock<IItemRepository>();
            _itemService = new ItemsService(_mockRepository.Object, _mockCategoryRepository.Object, _mockShelfRepository.Object, _mockMapper.Object);
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Tests the <see cref="ItemsService.GetAllAsync"/> method when items are retrieved successfully.
        /// </summary>

        [Fact]
        public async Task GetAllAsync_ReturnsItems()
        {
            // Arrange
            var items = new List<Item>
            {
                new Item { Id = 1, Name = "Medic 1", Description = " Description 1", Quantity = 15, DateEntered = DateTime.Now },
                new Item { Id = 2, Name = "Medic 2", Description = " Description 2", Quantity = 15, DateEntered = DateTime.Now }
            };

            var expectedItemsResponse = new List<ItemResponseDto>
            {
                new ItemResponseDto { Id = 1, Name = "Medic 1", Description = "Description 1", Quantity = 15, DateEntered = DateTime.Now },
                new ItemResponseDto { Id = 2, Name = "Medic 2", Description = "Description 2", Quantity = 15, DateEntered = DateTime.Now }
            };

            _mockRepository.Setup(repo => repo.GetAllItemsWithCategoryAndShelf()).ReturnsAsync(items);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ItemResponseDto>>(items)).Returns(expectedItemsResponse);

            // Act
            var result = await _itemService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Error);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
        }

        /// <summary>
        /// Tests the <see cref="ItemsService.GetAllAsync"/> method when no items exist.
        /// </summary>

        [Fact]
        public async Task GetAllAsync_ReturnsNotFound_WhenNoItemsExist()
        {
            // Arrange
            var items = new List<Item>();

            var expectedItemsResponse = new List<ItemResponseDto>();

            _mockRepository.Setup(repo => repo.GetAllItemsWithCategoryAndShelf()).ReturnsAsync(items);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ItemResponseDto>>(items)).Returns(expectedItemsResponse);

            // Act
            var result = await _itemService.GetAllAsync();

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        #endregion

        #region GetById Tests

        /// <summary>
        /// Tests the <see cref="ItemsService.GetByIdAsync"/> method when the item is not found.
        /// </summary>

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenItemNotFound()
        {
            // Arrange
            var itemId = 1;
            Item item = null;
            ItemResponseDto expectedItemsResponse = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);
            _mockMapper.Setup(mapper => mapper.Map<ItemResponseDto>(item)).Returns(expectedItemsResponse);

            // Act
            var result = await _itemService.GetByIdAsync(itemId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ItemsService.GetByIdAsync"/> method when the item is found.
        /// </summary>

        [Fact]
        public async Task GetByIdAsync_ReturnsItem_WhenItemFound()
        {
            // Arrange
            var itemId = 1;
            var item = new Item { Id = itemId, Name = "Medic 1", Description = " Description 1", Quantity = 15, DateEntered = DateTime.Now };
            var expectedItemsResponse = new ItemResponseDto { Id = itemId, Name = "Medic 1", Description = "Description 1", Quantity = 15, DateEntered = DateTime.Now };

            _mockRepository.Setup(repo => repo.GetItemByIdWithCategoryAndShelf(itemId)).ReturnsAsync(item);
            _mockMapper.Setup(mapper => mapper.Map<ItemResponseDto>(item)).Returns(expectedItemsResponse);

            // Act
            var result = await _itemService.GetByIdAsync(itemId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Tests the <see cref="ItemsService.CreateAsync"/> method when category is not found.
        /// </summary>

        [Fact]
        public async Task CreateAsync_ReturnCategoryNotFound_WhenCategoryIsNull()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "Medic 1", Description = "Description 1", Quantity = 15, CategoryId = 1, ShelfId = 1 };
            var item = new Item { Id = 1, Name = "Medic 1", Description = " Description 1", Quantity = 15, DateEntered = DateTime.Now };
            var expectedItemsResponse = new ItemResponseDto { Id = 1, Name = "Medic 1", Description = "Description 1", Quantity = 15, DateEntered = DateTime.Now };
            Category category = null;

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(createItemDto.CategoryId)).ReturnsAsync(category);

            // Act
            var result = await _itemService.CreateAsync(createItemDto);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ItemsService.CreateAsync"/> method when shelf is not found.
        /// </summary>

        [Fact]
        public async Task CreateAsync_ReturnShelfNotFound_WhenShelfIsNull()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "Medic 1", Description = "Description 1", Quantity = 15, CategoryId = 1, ShelfId = 1 };
            var item = new Item { Id = 1, Name = "Medic 1", Description = " Description 1", Quantity = 15, DateEntered = DateTime.Now };
            var expectedItemsResponse = new ItemResponseDto { Id = 1, Name = "Medic 1", Description = "Description 1", Quantity = 15, DateEntered = DateTime.Now };
            Shelf shelf = null;

            _mockShelfRepository.Setup(repo => repo.GetByIdAsync(createItemDto.ShelfId)).ReturnsAsync(shelf);

            // Act
            var result = await _itemService.CreateAsync(createItemDto);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ItemsService.CreateAsync"/> method when category and shelf is found.
        /// </summary>

        [Fact]
        public async Task CreateAsync_ShouldAddNewItem()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "Medic 1", Description = "Description 1", Quantity = 15, CategoryId = 1, ShelfId = 1 };
            var item = new Item { Id = 1, Name = "Medic 1", Description = " Description 1", Quantity = 15, DateEntered = DateTime.Now };
            var expectedItemsResponse = new ItemResponseDto { Id = 1, Name = "Medic 1", Description = "Description 1", Quantity = 15, DateEntered = DateTime.Now };
            Shelf shelf = new Shelf { Id = 1, ReferenceNumber = "A1"};
            Category category = new Category { Id = 1, Name = "Medic 1" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(createItemDto.CategoryId)).ReturnsAsync(category);
            _mockShelfRepository.Setup(repo => repo.GetByIdAsync(createItemDto.ShelfId)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map<Item>(createItemDto)).Returns(item);
            _mockMapper.Setup(mapper => mapper.Map<ItemResponseDto>(item)).Returns(expectedItemsResponse);

            // Act
            var result = await _itemService.CreateAsync(createItemDto);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Tests the <see cref="ItemsService.UpdateAsync"/> method when updating an item that category is not found.
        /// </summary>

        [Fact]
        public async Task UpdateAsync_ReturnNotFound_WhenNoCategoryFound()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "Medic 1", Description = "Description 1", Quantity = 15, CategoryId = 1, ShelfId = 1 };
            Category category = null;

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(createItemDto.CategoryId)).ReturnsAsync(category);

            // Act
            var result = await _itemService.CreateAsync(createItemDto);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ItemsService.UpdateAsync"/> method when updating an item when shelf is not found.
        /// </summary>

        [Fact]
        public async Task UpdateAsync_ReeturnsNotFound_WhenNoShelfFound()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "Medic 1", Description = "Description 1", Quantity = 15, CategoryId = 1, ShelfId = 1 };
            Shelf shelf = null;

            _mockShelfRepository.Setup(repo => repo.GetByIdAsync(createItemDto.ShelfId)).ReturnsAsync(shelf);

            // Act
            var result = await _itemService.CreateAsync(createItemDto);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ItemsService.UpdateAsync"/> method when updating an item when shelf and category is found.
        /// </summary>

        [Fact]
        public async Task UpdateAsync_ShouldUpdateItem_WhenCategoryAndShelfFound()
        {
            // Arrange
            var itemId = 1;
            var createItemDto = new CreateItemDto { Name = "Medic 2", Description = "Description 2", Quantity = 200, CategoryId = 1, ShelfId = 1 };
            var item = new Item { Id = 1, Name = "Medic 2", Description = " Description 2", Quantity = 200, DateEntered = DateTime.Now };
            var expectedItemsResponse = new ItemResponseDto { Id = 1, Name = "Medic 1", Description = "Description 1", Quantity = 15, DateEntered = DateTime.Now };
            Shelf shelf = new Shelf { Id = 1, ReferenceNumber = "A1" };
            Category category = new Category { Id = 1, Name = "Medic 1" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(createItemDto.CategoryId)).ReturnsAsync(category);
            _mockShelfRepository.Setup(repo => repo.GetByIdAsync(createItemDto.ShelfId)).ReturnsAsync(shelf);
            _mockRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);
            _mockMapper.Setup(mapper => mapper.Map(createItemDto, item)).Returns(item);
            _mockRepository.Setup(repo => repo.UpdateAsync(item)).ReturnsAsync(item);
            _mockMapper.Setup(mapper => mapper.Map<ItemResponseDto>(item)).Returns(expectedItemsResponse);

            // Act
            var result = await _itemService.UpdateAsync(createItemDto, itemId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Tests the <see cref="CategoryService.DeleteAsync"/> method when deleting an item that is not found.
        /// </summary>

        [Fact]
        public async Task DeleteAsync_ReturnNotFound_WhenNoItemFound()
        {
            // Arrange
            var itemId = 1;
            Item item = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);

            // Act
            var result = await _itemService.DeleteAsync(itemId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="CategoryService.DeleteAsync"/> method when deleting an item that is found.
        /// </summary>

        [Fact]
        public async Task DeleteAsync_ShouldDeleteItem_WhenItemFound()
        {
            // Arrange
            var itemId = 1;
            var item = new Item { Id = 1, Name = "Medic 1", Description = " Description 1", Quantity = 15, DateEntered = DateTime.Now };
            var expectedItemsResponse = new ItemResponseDto { Id = 1, Name = "Medic 1", Description = "Description 1", Quantity = 15, DateEntered = DateTime.Now };

            _mockRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);
            _mockRepository.Setup(repo => repo.DeleteAsync(item)).ReturnsAsync(item);
            _mockMapper.Setup(mapper => mapper.Map<ItemResponseDto>(item)).Returns(expectedItemsResponse);

            // Act
            var result = await _itemService.DeleteAsync(itemId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion
    }
}