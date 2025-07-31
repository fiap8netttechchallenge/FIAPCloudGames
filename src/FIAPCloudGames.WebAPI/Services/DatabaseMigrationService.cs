using FIAPCloudGames.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.WebAPI.Services;

public class DatabaseMigrationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseMigrationService> _logger;

    public DatabaseMigrationService(IServiceProvider serviceProvider, ILogger<DatabaseMigrationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FCGContext>();

        try
        {
            _logger.LogInformation("Iniciando migração automática do banco de dados...");

            await context.Database.MigrateAsync(cancellationToken);

            _logger.LogInformation("Migração do banco de dados concluída com sucesso!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a migração automática do banco de dados: {ErrorMessage}", ex.Message);

            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}