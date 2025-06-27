using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FIAPCloudGames.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGameService, GameService>();
        services.AddTransient<IJwtProvider, JwtProvider>();

        return services;
    }
}
