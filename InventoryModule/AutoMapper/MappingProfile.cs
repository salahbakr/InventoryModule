using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;

namespace InventoryModule.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Category, CategoryResponseDto>();
        }
    }
}
