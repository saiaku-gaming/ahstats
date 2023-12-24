using AHStats.gateways.models;

namespace AHStats.services;

public interface IAuctionService
{
    public Task<bool> CreateAuction(Auction auction);

    public Task<Auction?> GetAuction(string id);

    public Task<Auction?> GetLatestAuction();
}