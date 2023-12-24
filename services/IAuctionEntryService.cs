using AHStats.gateways.models;

namespace AHStats.services;

public interface IAuctionEntryService
{
    public Task<bool> CreateAuctionEntry(string auctionId, AuctionEntry auctionEntry);

    public Task<int> UpdateSoldAuctionEntries(string previousAuctionId, List<int> soldAuctionEntries);

    public Task<List<AuctionEntry>> GetAuctionEntriesFromAuctionId(string auctionId);

    public Task CreateAuctionEntries(IEnumerable<AuctionEntry> auctionEntries);
}