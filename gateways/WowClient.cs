using AHStats.gateways.models;
using AHStats.gateways.models.raw;

namespace AHStats.gateways;

public class WowClient(IHttpClientFactory httpClientFactory)
{
    private const string STATIC_NAMESPACE = "static-classic1x-eu";
    private const string DYNAMIC_NAMESPACE = "dynamic-classic1x-eu";
    private const string PROFILE_NAMESPACE = "profile-classic1x-eu";
    
    public async Task<RawAuctionsResponse> GetAuctions()
    {
        var httpClient = GetHttpClient();

        var response =
            await httpClient.GetAsync($"data/wow/connected-realm/5832/auctions/6?namespace={DYNAMIC_NAMESPACE}");

        return await response.Content.ReadFromJsonAsync<RawAuctionsResponse>();
    }

    public async Task<ItemData?> GetItemData(long id)
    {
        var httpClient = GetHttpClient();

        var response = await httpClient.GetAsync($"data/wow/item/{id}?namespace={STATIC_NAMESPACE}");

        if (!response.IsSuccessStatusCode) return null;
        
        var rawItem = await response.Content.ReadFromJsonAsync<RawItem>();

        return rawItem == null ? null : ItemData.Map(rawItem);
    }
    
    private HttpClient GetHttpClient() => httpClientFactory.CreateClient("WowApi");
}
