using AutoMapper;
using backend.Entities;
using backend.Dtos;

namespace backend.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Mapping for Customer -> CustomerReadDto
            CreateMap<Customer, CustomerReadDto>();

            // Mapping for CustomerCreateDto -> Customer
            CreateMap<CustomerCreateDto, Customer>();

            // Mapping for CustomerUpdateDto -> Customer
            CreateMap<CustomerUpdateDto, Customer>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
