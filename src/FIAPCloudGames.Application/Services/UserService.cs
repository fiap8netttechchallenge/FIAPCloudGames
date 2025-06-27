using AutoMapper;
using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Application.Services.Interfaces;
using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Extensions;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Domain.Repositores;
using System.Linq.Expressions;

namespace FIAPCloudGames.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserRepository userRepository,
        IJwtProvider jwtProvider)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<UserResponse> Create(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
        user.CreatedAt = DateTime.UtcNow;

        user.Password = Utils.Utils.HashPassword(request.Password);

        await _userRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<UserResponse>(user);
    }

    public async Task Update(UpdateUserRequest request)
    {
        var exists = await _userRepository.ExistAsync(x => x.Id == request.Id);
        if (!exists)
            throw new Exception("The user doesn't exist");

        Expression<Func<User, bool>> predicate = x => x.Id == request.Id;
        var userCurrent = await _userRepository.GetAsync(predicate);

        var user = _mapper.Map<User>(request);
        user.CreatedAt = userCurrent.CreatedAt;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task Delete(int id)
    {
        var exists = await _userRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new Exception("The user doesn't exist");

        Expression<Func<User, bool>> predicate = x => x.Id == id;
        var user = await _userRepository.GetAsync(predicate);

        await _userRepository.DeleteAsync(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<UserResponse>> GetAll()
    {
        Expression<Func<User, bool>> predicate = x => x.IsActive == true;
        var users = await _userRepository.GetAllAsync(predicate);

        var response = _mapper.Map<IEnumerable<UserResponse>>(users);

        return response;
    }

    public async Task<UserResponse?> GetById(int id)
    {
        Expression<Func<User, bool>> predicate = x => x.Id == id;
        var user = await _userRepository.GetAsync(predicate);

        if (user == null)
            return null;

        var response = _mapper.Map<UserResponse>(user);

        return response;
    }

    public async Task<UserResponse?> GetByEmail(string email)
    {
        Expression<Func<User, bool>> predicate = x => x.Email.ToLower().Equals(email.ToLower());
        var user = await _userRepository.GetAsync(predicate);

        if (user == null)
            return null;

        var response = _mapper.Map<UserResponse>(user);

        return response;
    }

    public async Task Active(int id)
    {
        var exists = await _userRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new Exception("The user doesn't exist.");

        Expression<Func<User, bool>> predicate = x => x.Id == id;
        var user = await _userRepository.GetAsync(predicate);

        user.IsActive = true;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task Inactive(int id)
    {
        var exists = await _userRepository.ExistAsync(x => x.Id == id);
        if (!exists)
            throw new Exception("The user doesn't exist.");

        Expression<Func<User, bool>> predicate = x => x.Id == id;
        var user = await _userRepository.GetAsync(predicate);

        user.IsActive = false;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        var user = await _userRepository.Login(request.Email, Utils.Utils.HashPassword(request.Password)) ?? throw new ArgumentException("The specified email or password are incorrect.");

        if (!user.IsActive)
        {
            throw new ArgumentException("The user is blocked!");
        }

        string token = _jwtProvider.GenerateToken(request.Email, user.UserType.GetDisplayName());

        return new TokenResponse(token, true);
    }
}
