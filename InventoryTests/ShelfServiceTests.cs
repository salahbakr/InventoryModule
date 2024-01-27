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

    /// <summary>
    /// Unit tests for the <see cref="ShelfService"/> class.
    /// </summary>

    public class ShelfServiceTests
    {

        private readonly IShelfService _shelfService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IShelfRepository> _mockRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShelfServiceTests"/> class.
        /// </summary>

        public ShelfServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IShelfRepository>();
            _shelfService = new ShelfService(_mockRepository.Object, _mockMapper.Object);
        }

        #region GetAllAsync Tests

        /// <summary>
        /// Tests the <see cref="ShelfService.GetAllAsync"/> method when shelfs are retrieved successfully.
        /// </summary>

        [Fact]
        public async Task GetAllAsync_ReturnsShelfsWithMessage()
        {
            // Arrange
            var shelfs = new List<Shelf>
            {
                new Shelf { Id = 1, ReferenceNumber = "A1" },
                new Shelf { Id = 2, ReferenceNumber = "A2" }
            };

            var expectedShelfsResponse = new List<ShelfResponseDto>
            {
                new ShelfResponseDto { Id = 1, ReferenceNumber = "A1" },
                new ShelfResponseDto { Id = 2, ReferenceNumber = "A2" }
            };

            _mockRepository.Setup(repo => repo.GetAllShelfsWithItems()).ReturnsAsync(shelfs);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ShelfResponseDto>>(shelfs)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        /// <summary>
        /// Tests the <see cref="ShelfService.GetAllAsync"/> method when no shelf exist.
        /// </summary>

        [Fact]
        public async Task GetAllAsync_ReturnsNotFound_WhenNoShelfsExist()
        {
            // Arrange
            var shelfs = new List<Shelf>();

            var expectedShelfsResponse = new List<ShelfResponseDto>();

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(shelfs);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ShelfResponseDto>>(shelfs)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.GetAllAsync();

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        #endregion

        #region GetById Tests

        /// <summary>
        /// Tests the <see cref="ShelfService.GetByIdAsync"/> method when the shelf is not found.
        /// </summary>

        [Fact]
        public async Task GetByIdAsync_ReturnsShelf_WhenShelfNotFound()
        {
            // Arrange
            var shelfId = 1;
            Shelf shelf = null;
            ShelfResponseDto expectedShelfsResponse = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(shelfId)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map<ShelfResponseDto>(shelf)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.GetByIdAsync(shelfId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ShelfService.GetByIdAsync"/> method when the shelf is found.
        /// </summary>

        [Fact]
        public async Task GetByIdAsync_ReturnsShelf_WhenShelfFound()
        {
            // Arrange
            var shelfId = 1;
            Shelf shelf = new Shelf { Id = shelfId, ReferenceNumber = "A1" };
            ShelfResponseDto expectedShelfResponse = new ShelfResponseDto { Id = shelfId, ReferenceNumber = "A1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(shelfId)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map<ShelfResponseDto>(shelf)).Returns(expectedShelfResponse);

            // Act
            var result = await _shelfService.GetByIdAsync(shelfId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Tests the <see cref="ShelfService.CreateAsync"/> method when creating a new shelf.
        /// </summary>

        [Fact]
        public async Task CreateAsync_ShouldAddNewCategory()
        {
            // Arrange
            CreateShelfDto createShelfDto = new CreateShelfDto { ReferenceNumber = "A1" };
            Shelf shelf = new Shelf { Id = 1, ReferenceNumber = "A1" };
            ShelfResponseDto expectedShelfsResponse = new ShelfResponseDto { Id = 1, ReferenceNumber = "A1" };

            _mockMapper.Setup(mapper => mapper.Map<Shelf>(createShelfDto)).Returns(shelf);
            _mockRepository.Setup(repo => repo.CreateAsync(shelf)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map<ShelfResponseDto>(shelf)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.CreateAsync(createShelfDto);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion

        #region UpdateAsync Tests

        /// <summary>
        /// Tests the <see cref="ShelfService.UpdateAsync"/> method when updating a shelf that is not found.
        /// </summary>

        [Fact]
        public async Task UpdateAsync_ReturnNotFound_WhenNoShelfFound()
        {
            // Arrange
            var shelfId = 1;
            CreateShelfDto createShelfDto = new CreateShelfDto { ReferenceNumber = "A1" };
            Shelf shelf = null;
            ShelfResponseDto expectedShelfsResponse = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(shelfId)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map(createShelfDto, shelf)).Returns(shelf);
            _mockMapper.Setup(mapper => mapper.Map<ShelfResponseDto>(shelf)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.UpdateAsync(createShelfDto, shelfId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ShelfService.UpdateAsync"/> method when updating a shelf that is found.
        /// </summary>

        [Fact]
        public async Task UpdateAsync_UpdateShelf_WhenShelfFound()
        {
            // Arrange
            var shelfId = 1;
            CreateShelfDto createShelfDto = new CreateShelfDto { ReferenceNumber = "A1" };
            Shelf shelf = new Shelf { Id = 1, ReferenceNumber = "A1" };
            ShelfResponseDto expectedShelfsResponse = new ShelfResponseDto { Id = shelfId, ReferenceNumber = "A1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(shelfId)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map(createShelfDto, shelf)).Returns(shelf);
            _mockRepository.Setup(repo => repo.UpdateAsync(shelf)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map<ShelfResponseDto>(shelf)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.UpdateAsync(createShelfDto, shelfId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion

        #region DeleteAsync Tests

        /// <summary>
        /// Tests the <see cref="ShelfService.DeleteAsync"/> method when deleting a shelf that is not found.
        /// </summary>

        [Fact]
        public async Task DeleteAsync_ReturnNotFound_WhenNoShelfFound()
        {
            // Arrange
            var shelfId = 1;
            CreateShelfDto createShelfDto = new CreateShelfDto { ReferenceNumber = "A1" };
            Shelf shelf = null;
            ShelfResponseDto expectedShelfsResponse = null;

            _mockRepository.Setup(repo => repo.GetByIdAsync(shelfId)).ReturnsAsync(shelf);
            _mockRepository.Setup(repo => repo.DeleteAsync(shelf)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map<ShelfResponseDto>(shelf)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.UpdateAsync(createShelfDto, shelfId);

            // Assert
            Assert.Null(result.Data);
            Assert.False(result.Success);
            Assert.NotNull(result.Error);
        }

        /// <summary>
        /// Tests the <see cref="ShelfService.DeleteAsync"/> method when deleting a shelf that is found.
        /// </summary>

        [Fact]
        public async Task DeleteAsync_ShouldDeleteShelf_WhenShelfFound()
        {
            // Arrange
            var shelfId = 1;
            CreateShelfDto createShelfDto = new CreateShelfDto { ReferenceNumber = "A1" };
            Shelf shelf = new Shelf { Id = 1, ReferenceNumber = "A1" };
            ShelfResponseDto expectedShelfsResponse = new ShelfResponseDto { Id = shelfId, ReferenceNumber = "A1" };

            _mockRepository.Setup(repo => repo.GetByIdAsync(shelfId)).ReturnsAsync(shelf);
            _mockRepository.Setup(repo => repo.DeleteAsync(shelf)).ReturnsAsync(shelf);
            _mockMapper.Setup(mapper => mapper.Map<ShelfResponseDto>(shelf)).Returns(expectedShelfsResponse);

            // Act
            var result = await _shelfService.UpdateAsync(createShelfDto, shelfId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.True(result.Success);
            Assert.Null(result.Error);
        }

        #endregion
    }
}