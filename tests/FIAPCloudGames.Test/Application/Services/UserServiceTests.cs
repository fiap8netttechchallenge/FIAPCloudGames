using AutoMapper;
using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Application.Services.Interfaces;
using FIAPCloudGames.Application.Utils;
using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enums;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Domain.Repositores;
using Moq;
using System.Linq.Expressions;

namespace FIAPCloudGames.Test.Application.Services;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;
    private readonly Mock<IJwtProvider> _jwtProviderMock;

    public UserServiceTests()
    {

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object, _userRepositoryMock.Object, _jwtProviderMock.Object);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Create_ShouldAddUser_AndReturnUserResponse()
    {
        var request = new CreateUserRequest { FullName = "Test", Login = "test", Password = "123", Email = "test@test.com", UserType = UserRole.Admin, IsActive = true };
        var user = new User { Id = 1, FullName = "Test", Login = "test", Password = "123", Email = "test@test.com", UserType = UserRole.Admin, IsActive = true, CreatedAt = DateTime.UtcNow };
        var response = new UserResponse { Id = 1, FullName = "Test", Login = "test", Email = "test@test.com", UserType = 1, IsActive = true, CreatedAt = user.CreatedAt };

        _mapperMock.Setup(m => m.Map<User>(request)).Returns(user);
        _userRepositoryMock.Setup(r => r.AddAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(response);

        var result = await _userService.Create(request);

        Assert.Equal(response.Id, result.Id);
        _userRepositoryMock.Verify(r => r.AddAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Update_ShouldThrowException_WhenUserDoesNotExist()
    {
        var request = new UpdateUserRequest { Id = 1 };
        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _userService.Update(request));
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Delete_ShouldThrowException_WhenUserDoesNotExist()
    {
        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _userService.Delete(1));
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task GetById_ShouldReturnUserResponse_WhenUserExists()
    {
        var user = new User { Id = 1, FullName = "Test", Login = "test", Email = "test@test.com", UserType = UserRole.Admin, IsActive = true, CreatedAt = DateTime.UtcNow };
        var response = new UserResponse { Id = 1, FullName = "Test", Login = "test", Email = "test@test.com", UserType = 1, IsActive = true, CreatedAt = user.CreatedAt };

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(response);

        var result = await _userService.GetById(1);

        Assert.Equal(response.Id, result.Id);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task GetByEmail_ShouldReturnUserResponse_WhenUserExists()
    {
        var user = new User { Id = 1, FullName = "Test", Login = "test", Email = "test@test.com", UserType = UserRole.Admin, IsActive = true, CreatedAt = DateTime.UtcNow };
        var response = new UserResponse { Id = 1, FullName = "Test", Login = "test", Email = "test@test.com", UserType = 1, IsActive = true, CreatedAt = user.CreatedAt };

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(response);

        var result = await _userService.GetByEmail("test@test.com");

        Assert.Equal(response.Id, result.Id);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Login_ShouldReturnTokenResponse_WhenCredentialsAreValid()
    {
        var request = new LoginRequest { Email = "test@test.com", Password = "123" };
        _userRepositoryMock.Setup(r => r.Login(request.Email, Utils.HashPassword("123"))).ReturnsAsync(new User() { Id = 1, IsActive = true });

        var result = await _userService.Login(request);

        Assert.True(result.Authenticated);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Login_ShouldThrowException_WhenCredentialsAreInvalid()
    {
        var request = new LoginRequest { Email = "test@test.com", Password = "wrong" };
        _userRepositoryMock.Setup(r => r.Login(request.Email, request.Password)).ReturnsAsync(new User() { Id = 1, IsActive = false });

        await Assert.ThrowsAsync<ArgumentException>(() => _userService.Login(request));
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Update_ShouldUpdateUser_WhenUserExists()
    {
        var request = new UpdateUserRequest
        {
            Id = 1,
            FullName = "Updated Name",
            Login = "updatedlogin",
            Password = "UpdatedPass1!",
            Email = "updated@email.com",
            UserType = UserRole.User,
            IsActive = true
        };
        var user = new User
        {
            Id = 1,
            FullName = "Old Name",
            Login = "oldlogin",
            Password = "OldPass1!",
            Email = "old@email.com",
            UserType = UserRole.Admin,
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<User>(request)).Returns(user);
        _userRepositoryMock.Setup(r => r.UpdateAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _userService.Update(request);

        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Delete_ShouldDeleteUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            FullName = "Test",
            Login = "test",
            Password = "123",
            Email = "test@test.com",
            UserType = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.DeleteAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _userService.Delete(user.Id);

        _userRepositoryMock.Verify(r => r.DeleteAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Active_ShouldSetUserAsActive_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            FullName = "Test",
            Login = "test",
            Password = "123",
            Email = "test@test.com",
            UserType = UserRole.Admin,
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.UpdateAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _userService.Active(user.Id);

        Assert.True(user.IsActive);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Inactive_ShouldSetUserAsInactive_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            FullName = "Test",
            Login = "test",
            Password = "123",
            Email = "test@test.com",
            UserType = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.UpdateAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CommitAsync(It.IsAny<bool>())).Returns(Task.CompletedTask);

        await _userService.Inactive(user.Id);

        Assert.False(user.IsActive);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task GetAll_ShouldReturnMappedUserResponses_WhenUsersExist()
    {
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                FullName = "Test",
                Login = "test",
                Password = "123",
                Email = "test@test.com",
                UserType = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        var userResponses = new List<UserResponse>
        {
            new UserResponse
            {
                Id = 1,
                FullName = "Test",
                Login = "test",
                Email = "test@test.com",
                UserType = 1,
                IsActive = true,
                CreatedAt = users[0].CreatedAt
            }
        };

        _userRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserResponse>>(users)).Returns(userResponses);

        var result = await _userService.GetAll();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(userResponses[0].Id, result.First().Id);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Active_ShouldThrowException_WhenUserDoesNotExist()
    {
        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<Exception>(() => _userService.Active(1));
        Assert.Equal("The user doesn't exist.", ex.Message);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task Inactive_ShouldThrowException_WhenUserDoesNotExist()
    {
        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<Exception>(() => _userService.Inactive(1));
        Assert.Equal("The user doesn't exist.", ex.Message);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task GetById_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var result = await _userService.GetById(1);

        Assert.Null(result);
    }

    [Fact]
    [Trait("Category", "UserService")]
    public async Task GetByEmail_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var result = await _userService.GetByEmail("Not existent mail");

        Assert.Null(result);
    }
}