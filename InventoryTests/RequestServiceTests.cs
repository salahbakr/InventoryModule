using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Enums;
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
    public class RequestServiceTests
    {

        private readonly Mock<IRequestRepository> _mockRequestRepository;
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IRequestService _requestService;

        /// <summary>
        /// Unit tests for the <see cref="RequestService"/> class.
        /// </summary>

        public RequestServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRequestRepository = new Mock<IRequestRepository>();
            _mockItemRepository = new Mock<IItemRepository>();
            _requestService = new RequestService(_mockRequestRepository.Object, _mockItemRepository.Object, _mockMapper.Object);
        }

        #region GetAll Tests

        /// <summary>
        /// Tests the <see cref="RequestService.GetAllAsync"/> method when the requests are not found.
        /// </summary>

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoRequestsFound()
        {
            // Arrange
            List<Request> requests = new List<Request>();

            _mockRequestRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(requests);

            // Act
            var result = await _requestService.GetAllAsync();

            // Assert
            Assert.NotNull(result.Error);
            Assert.False(result.Success);
        }

        /// <summary>
        /// Tests the <see cref="RequestService.GetAllAsync"/> method when the requests are found.
        /// </summary>

        [Fact]
        public async Task GetAll_ReturnsRequests_WhenRequestsFound()
        {
            // Arrange
            List<Request> requests = new List<Request> 
            {
                new Request { Id = 1, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending},
                new Request { Id = 2, DateExpected = DateTime.Now.AddDays(10), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending}
            };

            List<RequestResponseDto> expectedRequestsResponseDto = new List<RequestResponseDto> 
            {
                new RequestResponseDto { Id = 1, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending},
                new RequestResponseDto { Id = 2, DateExpected = DateTime.Now.AddDays(10), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending}
            };

            _mockRequestRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(requests);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<RequestResponseDto>>(requests)).Returns(expectedRequestsResponseDto);

            // Act
            var result = await _requestService.GetAllAsync();

            // Assert
            Assert.Null(result.Error);
            Assert.True(result.Success);
        }

        #endregion

        #region GetById Tests

        /// <summary>
        /// Tests the <see cref="RequestService.GetByIdAsync"/> method when the request is not found.
        /// </summary>

        [Fact]
        public async Task GetByIdAsync_ReturnNotFound_WhenNoRequestFound()
        {
            // Arrange
            var requestId = 1;
            Request request = null;

            _mockRequestRepository.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);

            // Act
            var result = await _requestService.GetByIdAsync(requestId);

            // Assert
            Assert.NotNull(result.Error);
            Assert.False(result.Success);
        }

        /// <summary>
        /// Tests the <see cref="RequestService.GetByIdAsync"/> method when the request is not found.
        /// </summary>

        [Fact]
        public async Task GetByIdAsync_ReturnsRequest_WhenRequestFound()
        {
            // Arrange
            var requestId = 1;
            Request request = new Request { Id = requestId, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending };
            RequestResponseDto expectedRequestResponseDto = new RequestResponseDto { Id = requestId, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending };

            _mockRequestRepository.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(request);
            _mockMapper.Setup(mapper => mapper.Map<RequestResponseDto>(request)).Returns(expectedRequestResponseDto);

            // Act
            var result = await _requestService.GetByIdAsync(requestId);

            // Assert
            Assert.Null(result.Error);
            Assert.True(result.Success);
        }

        #endregion

        #region CreateAsync Tests

        /// <summary>
        /// Tests the <see cref="RequestService.CreateAsync(CreateRequestDto)"/> method when the item in request is not found.
        /// </summary>

        [Fact]
        public async Task CreateAsync_ReturnsNotFound_WhenRequestItemNotFound()
        {
            // Arrange
            var itemId = 1;
            Item item = null;
            CreateRequestDto createRequestDto = new CreateRequestDto
            {
                From = "Pharmacy",
                DateExpected = DateTime.Now.AddDays(5),
                Status = Status.Pending,
                RequestItems = new List<CreateRequestItemsDto> { new CreateRequestItemsDto { ItemId = 1, Quantity = 20 } }
            };
            Request request = new Request { Id = 1, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending };
            RequestResponseDto expectedRequestResponseDto = new RequestResponseDto { Id = 1, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending };

            _mockMapper.Setup(mapper => mapper.Map<Request>(createRequestDto)).Returns(request);
            _mockItemRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);

            // Act
            var result = await _requestService.CreateAsync(createRequestDto);

            // Assert
            Assert.NotNull(result.Error);
            Assert.False(result.Success);
        }

        /// <summary>
        /// Tests the <see cref="RequestService.CreateAsync(CreateRequestDto)"/> method when the item quantity requested is not enough.
        /// </summary>

        [Fact]
        public async Task CreateAsync_ReturnsError_WhenItemQuantityNotSufficient()
        {
            // Arrange
            var requestId = 1;
            var itemId = 1;
            Item item = new Item { Id = 1, Name = "Medic 1", Description = "Description 1", Quantity = 20, DateEntered = DateTime.Now};
            CreateRequestDto createRequestDto = new CreateRequestDto
            {
                From = "Pharmacy",
                DateExpected = DateTime.Now.AddDays(5),
                Status = Status.Pending,
                RequestItems = new List<CreateRequestItemsDto> { new CreateRequestItemsDto { ItemId = 1, Quantity = 25 } }
            };
            Request request = new Request { Id = requestId, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending };
            RequestResponseDto expectedRequestResponseDto = new RequestResponseDto { Id = requestId, DateExpected = DateTime.Now.AddDays(5), From = "Pharmacy", RequestDate = DateTime.Now, Status = Status.Pending };

            _mockMapper.Setup(mapper => mapper.Map<Request>(createRequestDto)).Returns(request);
            _mockItemRepository.Setup(repo => repo.GetByIdAsync(itemId)).ReturnsAsync(item);

            // Act
            var result = await _requestService.CreateAsync(createRequestDto);

            // Assert
            Assert.NotNull(result.Error);
            Assert.False(result.Success);
        }

        #endregion
    }
}
