using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            // Mapping for CartItem -> CartItemReadDto
            CreateMap<CartItem, CartItemReadDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductSize.Product))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.ProductSize.Price))
                .ForMember(dest => dest.ProductSizeName, opt => opt.MapFrom(src => src.ProductSize.Size));

            // Mapping for CartItemCreateDto -> CartItem
            CreateMap<CartItemCreateDto, CartItem>();

            // Mapping for CartItemUpdateDto -> CartItem
            CreateMap<CartItemUpdateDto, CartItem>();
        }
    }
}
