namespace AHStats.gateways.auth;

public interface ITokenProvider
{
    Task<string> GetAccessTokenAsync();
}