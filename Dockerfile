# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY src/FIAPCloudGames.Domain/*.csproj ./src/FIAPCloudGames.Domain/
COPY src/FIAPCloudGames.Application/*.csproj ./src/FIAPCloudGames.Application/
COPY src/FIAPCloudGames.Infrastructure/*.csproj ./src/FIAPCloudGames.Infrastructure/
COPY src/FIAPCloudGames.WebAPI/*.csproj ./src/FIAPCloudGames.WebAPI/

# Restore dependencies
RUN dotnet restore src/FIAPCloudGames.WebAPI/FIAPCloudGames.WebAPI.csproj

# Copy source code
COPY src/ ./src/

# Build and publish
WORKDIR /app/src/FIAPCloudGames.WebAPI
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/publish .

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "FIAPCloudGames.WebAPI.dll"]