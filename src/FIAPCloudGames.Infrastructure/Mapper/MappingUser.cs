using AutoMapper;
using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Domain.Entities;

namespace FIAPCloudGames.Infrastructure.Mapper;

public class MappingUser : Profile
{
    public MappingUser()
    {
        CreateMap<CreateUserRequest, User>().ReverseMap();
        CreateMap<UpdateUserRequest, User>().ReverseMap();
        CreateMap<UserResponse, User>().ReverseMap();
        CreateMap<IEnumerable<UserResponse>, User>().ReverseMap();
    }
}