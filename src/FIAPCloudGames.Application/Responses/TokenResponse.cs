namespace FIAPCloudGames.Application.Responses;

public sealed class TokenResponse
{
    public TokenResponse(string token, bool authenticated)
    {
        Token = token;
        Authenticated = authenticated;
    }

    public string Token { get; }

    public bool Authenticated { get; }
}
