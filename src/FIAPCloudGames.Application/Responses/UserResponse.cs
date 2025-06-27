namespace FIAPCloudGames.Application.Responses;

public sealed class UserResponse
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int UserType { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
