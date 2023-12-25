using AHStats.gateways.models;

namespace AHStats.services;

public interface IAuctionEntryService
{
    public Task<int> UpdateSoldAuctionEntries(string previousAuctionId, List<int> soldAuctionEntries);

    public Task<List<AuctionEntry>> GetAuctionEntriesFromAuctionId(string auctionId);

    public Task CreateAuctionEntries(IEnumerable<AuctionEntry> auctionEntries);

    public Task<List<AuctionEntry>> GetSoldAuctionEntriesByActionIdAndItemIds(List<string> auctionIds, int itemId);
}