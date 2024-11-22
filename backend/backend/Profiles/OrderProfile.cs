using AutoMapper;
using backend.Dtos;
using backend.Entities;
using backend.Dtos;

namespace backend.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mapping for Order -> OrderReadDto
            CreateMap<Order, OrderReadDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"));

            // Mapping for OrderCreateDto -> Order
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OrderStatus.Pending));

            // Mapping for OrderUpdateDto -> Order
            CreateMap<OrderUpdateDto, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)));
        }
    }
}
