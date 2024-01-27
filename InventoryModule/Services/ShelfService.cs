using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Interfaces;

namespace InventoryModule.Services
{
    public class ShelfService : IShelfService
    {
        private readonly IShelfRepository _shelfRespository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShelfService"/> class.
        /// </summary>
        /// <param name="shelfRepository">The repository for shelf operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>

        public ShelfService(IShelfRepository shelfRepository, IMapper mapper)
        {
            _shelfRespository = shelfRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all shelfs asynchronously.
        /// </summary>
        /// <returns>A response model containing the list of shelfs.</returns>

        public async Task<ResponseModel<IEnumerable<ShelfResponseDto>>> GetAllAsync()
        {
            var shelfs = await _shelfRespository.GetAllShelfsWithItems();

            if (shelfs.Count() == 0)
            {
                return new ResponseModel<IEnumerable<ShelfResponseDto>>
                {
                    Message = "There are no shelfs",
                    Error = "Error while retrieving shelfs"
                };
            }

            var shelfsDto = _mapper.Map<IEnumerable<ShelfResponseDto>>(shelfs);

            return new ResponseModel<IEnumerable<ShelfResponseDto>> { Message = "Retrieved shelfs", Data = shelfsDto };
        }

        /// <summary>
        /// Gets a shelf by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the shelf.</param>
        /// <returns>A response model containing the retrieved shelf.</returns>

        public async Task<ResponseModel<ShelfResponseDto>> GetByIdAsync(int id)
        {
            var shelf = await _shelfRespository.GetByIdAsync(id);

            if (shelf is null)
            {
                return NotFoundShelfError();
            }

            return new ResponseModel<ShelfResponseDto>
            {
                Message = "Retrieved shelf",
                Data = _mapper.Map<ShelfResponseDto>(shelf)
            };
        }

        /// <summary>
        /// Creates a new shelf asynchronously.
        /// </summary>
        /// <param name="shelfDto">The data transfer object for creating a shelf.</param>
        /// <returns>A response model containing the created shelf.</returns>

        public async Task<ResponseModel<ShelfResponseDto>> CreateAsync(CreateShelfDto shelfDto)
        {
            var shelf = _mapper.Map<Shelf>(shelfDto);

            await _shelfRespository.CreateAsync(shelf);

            return new ResponseModel<ShelfResponseDto>
            {
                Message = "Successfully added a new shelf",
                Data = _mapper.Map<ShelfResponseDto>(shelf)
            };
        }

        /// <summary>
        /// Updates an existing shelf asynchronously.
        /// </summary>
        /// <param name="shelfDto">The data transfer object for updating a shelf.</param>
        /// <param name="id">The ID of the shelf to update.</param>
        /// <returns>A response model containing the updated shelf.</returns>

        public async Task<ResponseModel<ShelfResponseDto>> UpdateAsync(CreateShelfDto shelfDto, int id)
        {
            var shelf = await _shelfRespository.GetByIdAsync(id);

            if (shelf is null)
            {
                return NotFoundShelfError();
            }

            _mapper.Map(shelfDto, shelf);

            await _shelfRespository.UpdateAsync(shelf);

            return new ResponseModel<ShelfResponseDto>
            {
                Message = "Successfully updated shelf",
                Data = _mapper.Map<ShelfResponseDto>(shelf)
            };
        }

        /// <summary>
        /// Deletes a shelf by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the shelf to delete.</param>
        /// <returns>A response model containing the deleted shelf.</returns>

        public async Task<ResponseModel<ShelfResponseDto>> DeleteAsync(int id)
        {
            var shelf = await _shelfRespository.GetByIdAsync(id);

            if (shelf is null)
            {
                return NotFoundShelfError();
            }

            await _shelfRespository.DeleteAsync(shelf);

            return new ResponseModel<ShelfResponseDto>
            {
                Message = "Successfully removed shelf",
                Data = _mapper.Map<ShelfResponseDto>(shelf)
            };
        }

        /// <summary>
        /// Handles the case when a shelf is not found.
        /// </summary>
        /// <returns>A response model indicating a shelf not found error.</returns>

        internal ResponseModel<ShelfResponseDto> NotFoundShelfError()
        {
            return new ResponseModel<ShelfResponseDto>
            {
                Message = "Can not find a shelf with that id",
                Error = "Error while retrieving shelf"
            };
        }
    }
}
