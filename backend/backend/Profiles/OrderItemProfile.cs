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
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductSize.ProductID))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductSize.Product.Name)) 
                .ForMember(dest => dest.ProductSizeName, opt => opt.MapFrom(src => src.ProductSize.Size));

            // Mapping for OrderItemCreateDto -> OrderItem
            CreateMap<OrderItemCreateDto, OrderItem>();

            // Mapping for OrderItemUpdateDto -> OrderItem
            CreateMap<OrderItemUpdateDto, OrderItem>();
        }
    }
}
