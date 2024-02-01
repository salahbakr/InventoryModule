using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;

namespace InventoryModule.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Category, CategoryResponseDto>();

            // Shelf mappings
            CreateMap<CreateShelfDto, Shelf>();

            CreateMap<Shelf, ShelfResponseDto>();

            // Item mappings
            CreateMap<CreateItemDto, Item>();

            CreateMap<Item, ItemResponseDto>();

            // Request mappings
            CreateMap<CreateRequestDto, Request>();

            CreateMap<Request, RequestResponseDto>();

            CreateMap<CreateRequestItemsDto, RequestItem>();

            CreateMap<RequestItem, RequestItemResponseDto>();

            // Order mappings
            CreateMap<Order, OrderResponseDto>();
        }
    }
}