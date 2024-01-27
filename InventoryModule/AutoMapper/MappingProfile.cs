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
        }
    }
}