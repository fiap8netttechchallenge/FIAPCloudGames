namespace FIAPCloudGames.Application.Services.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(string userName, string role);
}
