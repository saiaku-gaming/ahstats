using AHStats.gateways.models;

namespace AHStats.services;

public interface IAuctionEntryService
{
    public Task<bool> CreateAuctionEntry(string auctionId, AuctionEntry auctionEntryService);

    public Task<int> UpdateSoldAuctionEntries(string previousAuctionId, List<long> soldAuctionEntries);

    public Task<List<AuctionEntry>> GetAuctionEntriesFromAuctionId(string auctionId);
}