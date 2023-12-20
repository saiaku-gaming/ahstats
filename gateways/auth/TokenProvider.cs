using System.Net.Http.Headers;
using AHStats.options;
using Microsoft.Extensions.Options;

namespace AHStats.gateways.auth;

public class TokenProvider(IHttpClientFactory httpClientFactory, IOptions<WowClientOptions> options) : ITokenProvider
{
    private readonly WowClientOptions _options = options.Value;

    public async Task<string> GetAccessTokenAsync()
    {
        var httpClient = httpClientFactory.CreateClient("BNetOAuth");

        var auth = $"{_options.ClientId}:{_options.ClientSecret}";
        var authB64 = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(auth));

        var values = new List<KeyValuePair<string, string>> { new("grant_type", "client_credentials") };
        var content = new FormUrlEncodedContent(values);
        
        var request = new HttpRequestMessage(HttpMethod.Post, "token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authB64);
        request.Content = content;


        var response = await httpClient.SendAsync(request);
        
        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

        return tokenResponse.access_token;
    }
}