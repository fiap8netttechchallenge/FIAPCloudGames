using AutoMapper;
using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Domain.Entities;

namespace FIAPCloudGames.Infrastructure.Mapper;

public class MappingGame : Profile
{
    public MappingGame()
    {
        CreateMap<CreateGameRequest, Game>().ReverseMap();
        CreateMap<UpdateGameRequest, Game>().ReverseMap();
        CreateMap<GameResponse, Game>().ReverseMap();
        CreateMap<IEnumerable<GameResponse>, Game>().ReverseMap();
    }
}