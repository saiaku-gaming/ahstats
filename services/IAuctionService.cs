using AHStats.gateways.models;

namespace AHStats.services;

public interface IAuctionService
{
    public Task<bool> CreateAuction(Auction auction);

    public Task<Auction?> GetAuction(string id);

    public Task<Auction?> GetLatestAuction(AuctionHouse auctionHouse);

    public Task<List<Auction>> GetAuctionByAge(int hours, int auctionHouse);

    public Task<bool> DeleteAuction(string id);
}