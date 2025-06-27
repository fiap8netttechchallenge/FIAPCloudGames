namespace FIAPCloudGames.Application.Responses;

public sealed class GameResponse
{
    public int Id { get; set; }  

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
