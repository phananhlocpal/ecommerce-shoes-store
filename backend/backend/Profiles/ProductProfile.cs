using AutoMapper;
using backend.Entities;
using backend.Dtos;

namespace backend.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            // Mapping for Product -> ProductReadDto
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name)); // Assuming Category has Name

            // Mapping for ProductCreateDto -> Product
            CreateMap<ProductCreateDto, Product>();

            // Mapping for ProductUpdateDto -> Product
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
