namespace FIAPCloudGames.Application.Requests;

public sealed class CreateGameRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }
}
