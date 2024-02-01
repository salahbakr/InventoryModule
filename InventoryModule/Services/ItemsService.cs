using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Helpers;
using InventoryModule.Interfaces;

namespace InventoryModule.Services
{
    public class ItemsService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Shelf> _shelfRepository;
        private readonly IMapper _mapper;

        public ItemsService(IItemRepository itemRepository, IGenericRepository<Category> categoryRepository, IGenericRepository<Shelf> shelfRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
            _shelfRepository = shelfRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all items asynchronously.
        /// </summary>
        /// <returns>A response model containing the list of items.</returns>

        public async Task<ResponseModel<IEnumerable<ItemResponseDto>>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllItemsWithCategoryAndShelf();

            if (items.Count() == 0)
            {
                return new ResponseModel<IEnumerable<ItemResponseDto>>
                {
                    Message = "There are no items",
                    Error = "Error while retrieving items"
                };
            }

            var itemsDto = _mapper.Map<IEnumerable<ItemResponseDto>>(items);

            return new ResponseModel<IEnumerable<ItemResponseDto>> { Message = "Retrieved categories", Data = itemsDto };
        }

        /// <summary>
        /// Gets a item by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <returns>A response model containing the retrieved item.</returns>

        public async Task<ResponseModel<ItemResponseDto>> GetByIdAsync(int id)
        {
            var item = await _itemRepository.GetItemByIdWithCategoryAndShelf(id);

            if (item is null)
            {
                return NotFoundItemError();
            }

            return new ResponseModel<ItemResponseDto>
            {
                Message = "Retrieved item",
                Data = _mapper.Map<ItemResponseDto>(item)
            };
        }

        /// <summary>
        /// Creates a new item asynchronously.
        /// </summary>
        /// <param name="itemDto">The data transfer object for creating an item.</param>
        /// <returns>A response model containing the created item.</returns>

        public async Task<ResponseModel<ItemResponseDto>> CreateAsync(CreateItemDto itemDto)
        {
            var category = await _categoryRepository.GetByIdAsync(itemDto.CategoryId);

            if (category is null)
            {
                return new ResponseModel<ItemResponseDto>
                {
                    Error = "Error while validating category",
                    Message = "Category is not found."
                };
            }        
            
            var shelf = await _shelfRepository.GetByIdAsync(itemDto.ShelfId);

            if (shelf is null)
            {
                return new ResponseModel<ItemResponseDto>
                {
                    Error = "Error while validating shelf",
                    Message = "shelf is not found."
                };
            }

            var item = _mapper.Map<Item>(itemDto);

            item.Category = category;  // Assign the category to the item.
            item.Shelf = shelf;  // Assign the shelf to the item.

            await _itemRepository.CreateAsync(item);

            return new ResponseModel<ItemResponseDto>
            {
                Message = "Successfully added a new item",
                Data = _mapper.Map<ItemResponseDto>(item)
            };
        }

        /// <summary>
        /// Updates an existing item asynchronously.
        /// </summary>
        /// <param name="itemDto">The data transfer object for updating an item.</param>
        /// <param name="id">The ID of the item to update.</param>
        /// <returns>A response model containing the updated item.</returns>

        public async Task<ResponseModel<ItemResponseDto>> UpdateAsync(CreateItemDto itemDto, int id)
        {
            var category = await _categoryRepository.GetByIdAsync(itemDto.CategoryId);

            if (category is null)
            {
                return new ResponseModel<ItemResponseDto>
                {
                    Error = "Error while validating category",
                    Message = "Category is not found."
                };
            }

            var shelf = await _shelfRepository.GetByIdAsync(itemDto.ShelfId);

            if (shelf is null)
            {
                return new ResponseModel<ItemResponseDto>
                {
                    Error = "Error while validating shelf",
                    Message = "shelf is not found."
                };
            }

            var item = await _itemRepository.GetByIdAsync(id);

            if (item is null)
            {
                return NotFoundItemError();
            }

            _mapper.Map(itemDto, item);  // Merge the updated item with the existing item.

            item.Category = category;  // Assign the category to the item.
            item.Shelf = shelf;  // Assign the shelf to the item.

            await _itemRepository.UpdateAsync(item);

            return new ResponseModel<ItemResponseDto>
            {
                Message = "Successfully updated item",
                Data = _mapper.Map<ItemResponseDto>(item)
            };
        }

        /// <summary>
        /// Deletes an item by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>A response model containing the deleted item.</returns>

        public async Task<ResponseModel<ItemResponseDto>> DeleteAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);

            if (item is null)
            {
                return NotFoundItemError();
            }

            await _itemRepository.DeleteAsync(item);

            return new ResponseModel<ItemResponseDto>
            {
                Message = "Successfully removed category",
                Data = _mapper.Map<ItemResponseDto>(item)
            };
        }

        /// <summary>
        /// Changes items quantities depending on the operation whether it is subtraction or addition.
        /// </summary>
        /// <param name="items">List of items to operate on</param>
        /// <param name="requestedItems">List of the requested items with the quantity to subtract or add</param>
        /// <param name="operation">Math operation to determine if it is subtraction or addition</param>
        /// <returns> Items with the new quantities </returns>

        public IEnumerable<Item> ChangeItemsQuantities(IEnumerable<Item> items, List<CreateRequestItemsDto> requestedItems, string operation)
        {
            foreach (var item in items)
            {
                if (operation == MathOperation.Subtraction)
                {
                    item.Quantity = item.Quantity - requestedItems.Where(requestedItems => requestedItems.ItemId == item.Id)
                                              .Select(requestedItem => requestedItem.Quantity).Single();
                }
                else
                {
                    item.Quantity = item.Quantity + requestedItems.Where(requestedItems => requestedItems.ItemId == item.Id)
                                              .Select(requestedItem => requestedItem.Quantity).Single();
                }
            }

            return items;
        }

        /// <summary>
        /// Handles the case when an item is not found.
        /// </summary>
        /// <returns>A response model indicating a item not found error.</returns>

        internal ResponseModel<ItemResponseDto> NotFoundItemError()
        {
            return new ResponseModel<ItemResponseDto>
            {
                Message = "Can not find an item with that id",
                Error = "Error while retrieving item"
            };
        }
    }
}
