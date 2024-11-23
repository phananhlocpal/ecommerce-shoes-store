using AutoMapper;
using backend.Dtos;
using backend.Entities;

namespace backend.Profiles 
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            // Mapping for OrderItem -> OrderItemReadDto
            CreateMap<OrderItem, OrderItemReadDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductSize.Product))
                .ForMember(dest => dest.ProductSize, opt => opt.MapFrom(src => src.ProductSize));

            // Mapping for OrderItemCreateDto -> OrderItem
            CreateMap<OrderItemCreateDto, OrderItem>();

            // Mapping for OrderItemUpdateDto -> OrderItem
            CreateMap<OrderItemUpdateDto, OrderItem>();
        }
    }
}
