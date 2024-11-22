using AutoMapper;
using backend.Entities;
using backend.Dtos;

namespace backend.Profiles
{
    public class ProductSizeProfile : Profile
    {
        public ProductSizeProfile()
        {
            // Mapping for ProductSize -> ProductSizeReadDto
            CreateMap<ProductSize, ProductSizeReadDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name)); // Assuming Product has Name

            // Mapping for ProductSizeCreateDto -> ProductSize
            CreateMap<ProductSizeCreateDto, ProductSize>();

            // Mapping for ProductSizeUpdateDto -> ProductSize
            CreateMap<ProductSizeUpdateDto, ProductSize>();
        }
    }
}
