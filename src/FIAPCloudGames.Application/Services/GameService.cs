using AutoMapper;
using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Application.Services.Interfaces;
using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Domain.Repositores;
using System.Linq.Expressions;

namespace FIAPCloudGames.Application.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IGameRepository _gameRepository;
    private readonly IJwtProvider _jwtProvider;

    public GameService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IGameRepository gameRepository,
        IJwtProvider jwtProvider)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
        _jwtProvider = jwtProvider;
    }


    public async Task<GameResponse> Create(CreateGameRequest request)
    {
        var game = _mapper.Map<Game>(request);
        game.CreatedAt = DateTime.UtcNow;

        await _gameRepository.AddAsync(game);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<GameResponse>(game);
    }

    public async Task Update(UpdateGameRequest request)
    {
        var exists = await _gameRepository.ExistAsync(x => x.Id == request.Id);
        if (!exists)
            throw new Exception("The game doesn't exist");

        Expression<Func<Game, bool>> predicate = x => x.Id == request.Id;
        var gameCurrent = await _gameRepository.GetAsync(predicate);

        var game = _mapper.Map<Game>(request);
        game.CreatedAt = gameCurrent.CreatedAt;

        await _gameRepository.UpdateAsync(game);
        await _unitOfWork.CommitAsync();
    }

    public async Task Delete(int id)
    {
        var exists = await _gameRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new Exception("The game doesn't exist");

        Expression<Func<Game, bool>> predicate = x => x.Id == id;
        var game = await _gameRepository.GetAsync(predicate);

        await _gameRepository.DeleteAsync(game);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<GameResponse>> GetAll()
    {
        Expression<Func<Game, bool>> predicate = x => x.IsActive == true;

        var games = await _gameRepository.GetAllAsync(predicate);

        var response = _mapper.Map<IEnumerable<GameResponse>>(games);

        return response;
    }

    public async Task<GameResponse> GetById(int id)
    {
        var exists = await _gameRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new Exception("The game doesn't exist.");

        Expression<Func<Game, bool>> predicate = x => x.Id == id;
        var user = await _gameRepository.GetAsync(predicate);

        var response = _mapper.Map<GameResponse>(user);

        return response;
    }

    public async Task Active(int id)
    {
        var exists = await _gameRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new Exception("The game doesn't exist.");

        Expression<Func<Game, bool>> predicate = x => x.Id == id;
        var game = await _gameRepository.GetAsync(predicate);

        game.IsActive = true;

        await _gameRepository.UpdateAsync(game);
        await _unitOfWork.CommitAsync();
    }

    public async Task Inactive(int id)
    {
        var exists = await _gameRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new Exception("The game doesn't exist.");

        Expression<Func<Game, bool>> predicate = x => x.Id == id;
        var game = await _gameRepository.GetAsync(predicate);

        game.IsActive = false;

        await _gameRepository.UpdateAsync(game);
        await _unitOfWork.CommitAsync();
    }
}
