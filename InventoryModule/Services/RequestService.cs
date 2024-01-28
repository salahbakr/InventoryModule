using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Enums;
using InventoryModule.Interfaces;

namespace InventoryModule.Services
{
    /// <summary>
    /// Service class for managing categories.
    /// </summary>

    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestService"/> class.
        /// </summary>
        /// <param name="requestRepository">The repository for request operations.</param>
        /// <param name="itemRepository">The repository for item operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>

        public RequestService(IRequestRepository requestRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _requestRepository = requestRepository;
        }


        /// <summary>
        /// Gets all requests with items included asynchronously.
        /// </summary>
        /// <returns>A response model containing the list of requests each with its items.</returns>

        public async Task<ResponseModel<IEnumerable<RequestResponseDto>>> GetAllAsync()
        {
            var requests = await _requestRepository.GetAllAsync();

            if (requests.Count() == 0)
            {
                return new ResponseModel<IEnumerable<RequestResponseDto>>
                {
                    Message = "There are no requests",
                    Error = "Error while retrieving requests"
                };
            }

            var requestsDto = _mapper.Map<IEnumerable<RequestResponseDto>>(requests);

            return new ResponseModel<IEnumerable<RequestResponseDto>> { Message = "Retrieved requests", Data = requestsDto };
        }

        /// <summary>
        /// Gets a request by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the request.</param>
        /// <returns>A response model containing the retrieved request with ites items.</returns>

        public async Task<ResponseModel<RequestResponseDto>> GetByIdAsync(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);

            if (request is null)
            {
                return NotFoundRequestError();
            }

            return new ResponseModel<RequestResponseDto>
            {
                Message = "Retrieved request",
                Data = _mapper.Map<RequestResponseDto>(request)
            };
        }

        /// <summary>
        /// Creates a new request asynchronously.
        /// </summary>
        /// <param name="requestDto">The data transfer object for creating a request with items.</param>
        /// <returns>A response model containing the created request.</returns>

        public async Task<ResponseModel<RequestResponseDto>> CreateAsync(CreateRequestDto requestDto)
        {
            var request = _mapper.Map<Request>(requestDto);

            List<RequestItem> requestedItems = new List<RequestItem>();

            foreach (var itemRequestedDto in requestDto.RequestItems)
            {
                var item = await _itemRepository.GetByIdAsync(itemRequestedDto.ItemId);

                if (item is null)
                {
                    return new ResponseModel<RequestResponseDto>
                    {
                        Error = "Error while retrieving item",
                        Message = "cAn not find item with id " + itemRequestedDto.ItemId
                    };
                }

                item.Quantity = item.Quantity - itemRequestedDto.Quantity;

                if (item.Quantity < 0)
                {
                    return new ResponseModel<RequestResponseDto>
                    {
                        Error = "Error while creating a request",
                        Message = "Insufficient amount of item " + item.Name
                    };
                }


                //var requestedItem = _mapper.Map<RequestItem>(itemRequestedDto);
                requestedItems.Add(
                    new RequestItem
                    {
                        Quantity = itemRequestedDto.Quantity,
                        Item = item,
                        Request = request
                    });
            }

            request.RequestItems = requestedItems;

            await _requestRepository.CreateAsync(request);

            return new ResponseModel<RequestResponseDto>
            {
                Message = "Successfully added a new request",
                Data = _mapper.Map<RequestResponseDto>(request)
            };
        }

        /// <summary>
        /// Changes an existing request status asynchronously.
        /// </summary>
        /// <param name="id">The ID of the request tochange its status.</param>
        /// <returns>A response model containing the request.</returns>

        public async Task<ResponseModel<RequestResponseDto>> ChangeRequestStatus(int id, Status status)
        {
            var request = await _requestRepository.GetByIdAsync(id);

            if (request is null)
            {
                return NotFoundRequestError();
            }

            if (status == Status.Canceled)
            {
                foreach (var requestItem in request.RequestItems)
                {
                    requestItem.Item.Quantity = requestItem.Item.Quantity + requestItem.Quantity;
                }
            }

            request.Status = status;

            await _requestRepository.UpdateAsync(request);

            return new ResponseModel<RequestResponseDto>
            {
                Message = "Successfully updated request",
                Data = _mapper.Map<RequestResponseDto>(request)
            };
        }

        /// <summary>
        /// Handles the case when a request is not found.
        /// </summary>
        /// <returns>A response model indicating a request not found error.</returns>

        internal ResponseModel<RequestResponseDto> NotFoundRequestError()
        {
            return new ResponseModel<RequestResponseDto>
            {
                Message = "Can not find a request with that id",
                Error = "Error while retrieving request"
            };
        }
    }
}
