using AutoMapper;
using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Application.Services.Interfaces;
using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Domain.Repositores;
using Moq;
using System.Linq.Expressions;

namespace FIAPCloudGames.Test.Application.Services;

public class GameServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;

    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _jwtProviderMock = new Mock<IJwtProvider>();

        _gameRepositoryMock = new Mock<IGameRepository>();
        _gameService = new GameService(_unitOfWorkMock.Object, _mapperMock.Object, _gameRepositoryMock.Object, _jwtProviderMock.Object);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Create_ShouldAddGameActive_AndReturnGameResponse()
    {
        var gameRequest = new CreateGameRequest { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, UserId = 1, IsActive = true };
        var game = new Game { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };
        var gameResponse = new GameResponse { Id = 1, Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = game.CreatedAt, UserId = 1 };

        _mapperMock.Setup(m => m.Map<Game>(gameRequest)).Returns(game);
        _gameRepositoryMock.Setup(r => r.AddAsync(game, new CancellationToken())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<GameResponse>(game)).Returns(gameResponse);

        var result = await _gameService.Create(gameRequest);

        Assert.Equal(gameResponse.Id, result.Id);
        _gameRepositoryMock.Verify(r => r.AddAsync(game, new CancellationToken()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }


    [Fact]
    [Trait("Category", "GameService")]
    public async Task Create_ShouldAddGameInactive_AndReturnGameResponse()
    {
        var gameRequest = new CreateGameRequest { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, UserId = 1, IsActive = false };
        var game = new Game { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };
        var gameResponse = new GameResponse { Id = 1, Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = false, CreatedAt = game.CreatedAt, UserId = 1 };

        _mapperMock.Setup(m => m.Map<Game>(gameRequest)).Returns(game);
        _gameRepositoryMock.Setup(r => r.AddAsync(game, new CancellationToken())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<GameResponse>(game)).Returns(gameResponse);

        var result = await _gameService.Create(gameRequest);

        Assert.Equal(gameResponse.Id, result.Id);
        _gameRepositoryMock.Verify(r => r.AddAsync(game, new CancellationToken()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Update_ShouldThrowException_WhenGameDoesNotExist()
    {
        var request = new UpdateGameRequest { Id = 1 };
        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _gameService.Update(request));
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Delete_ShouldThrowException_WhenGameDoesNotExist()
    {
        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _gameService.Delete(1));
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task GetById_ShouldReturnGameResponse_WhenGameExists()
    {
        var game = new Game { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };
        var response = new GameResponse { Id = 1, Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = game.CreatedAt, UserId = 1 };

        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(true);
        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(game);
        _mapperMock.Setup(m => m.Map<GameResponse>(game)).Returns(response);

        var result = await _gameService.GetById(1);

        Assert.Equal(response.Id, result.Id);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Update_ShouldUpdateGame_WhenGameExists()
    {
        var request = new UpdateGameRequest { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, UserId = 1, IsActive = true };
        var game = new Game { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };

        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(true);
        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(game);
        _mapperMock.Setup(m => m.Map<Game>(request)).Returns(game);
        _gameRepositoryMock.Setup(r => r.UpdateAsync(game, new CancellationToken())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _gameService.Update(request);

        _gameRepositoryMock.Verify(r => r.UpdateAsync(game, new CancellationToken()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Delete_ShouldDeleteGame_WhenGameExists()
    {
        var game = new Game { Id = 1, Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };

        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(true);
        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(game);
        _gameRepositoryMock.Setup(r => r.DeleteAsync(game, new CancellationToken())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _gameService.Delete(game.Id);

        _gameRepositoryMock.Verify(r => r.DeleteAsync(game, new CancellationToken()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Active_ShouldSetGameAsActive_WhenGameExists()
    {
        var game = new Game { Id = 1, Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };

        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(true);
        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(game);
        _gameRepositoryMock.Setup(r => r.UpdateAsync(game, new CancellationToken())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _gameService.Active(game.Id);

        Assert.True(game.IsActive);
        _gameRepositoryMock.Verify(r => r.UpdateAsync(game, new CancellationToken()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Inactive_ShouldSetGameAsInactive_WhenGameExists()
    {
        var game = new Game { Id = 1, Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };

        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(true);
        _gameRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(game);
        _gameRepositoryMock.Setup(r => r.UpdateAsync(game, new CancellationToken())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _gameService.Inactive(game.Id);

        Assert.False(game.IsActive);
        _gameRepositoryMock.Verify(r => r.UpdateAsync(game, new CancellationToken()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task GetAll_ShouldReturnMappedGameResponses_WhenGamesExist()
    {
        var games = new List<Game>();
        var game = new Game { Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = DateTime.UtcNow, UserId = 1 };
        games.Add(game);

        var gameResponses = new List<GameResponse>();
        var gameResponse = new GameResponse { Id = 1, Name = "Fifa game test", Description = "Description Fifa game test", Company = "EA Sports FC", Price = 542.50M, IsActive = true, CreatedAt = game.CreatedAt, UserId = 1 };
        gameResponses.Add(gameResponse);

        _gameRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(games);
        _mapperMock.Setup(m => m.Map<IEnumerable<GameResponse>>(games)).Returns(gameResponses);

        var result = await _gameService.GetAll();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(gameResponses[0].Id, result.First().Id);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Active_ShouldThrowException_WhenGameDoesNotExist()
    {
        await Create_ShouldAddGameActive_AndReturnGameResponse();

        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<Exception>(() => _gameService.Active(1));
        Assert.Equal("The game doesn't exist.", ex.Message);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task Inactive_ShouldThrowException_WhenGameDoesNotExist()
    {
        await Create_ShouldAddGameInactive_AndReturnGameResponse();

        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<Exception>(() => _gameService.Inactive(1));
        Assert.Equal("The game doesn't exist.", ex.Message);
    }

    [Fact]
    [Trait("Category", "GameService")]
    public async Task GetById_ShouldThrowException_WhenGameDoesNotExist()
    {
        _gameRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<Game, bool>>>(), new CancellationToken())).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<Exception>(() => _gameService.GetById(1));
        Assert.Equal("The game doesn't exist.", ex.Message);
    }
}