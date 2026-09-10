namespace DingFood.Application.Abstractions.Integrations.Keeta
{
    public sealed record KeetaTokenResponse(string AccessToken, string TokenType, int ExpiresIn);
}
