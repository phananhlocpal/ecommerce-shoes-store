using AutoMapper;
using backend.Entities;
using backend.Dtos;

namespace backend.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapping from User -> UserReadDto
            CreateMap<User, UserReadDto>();

            // Mapping from UserCreateDto -> User
            CreateMap<UserCreateDto, User>();

            // Mapping from UserUpdateDto -> User
            CreateMap<UserUpdateDto, User>();
        }
    }
}
