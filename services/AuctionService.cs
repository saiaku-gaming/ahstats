using AHStats.gateways.models;

namespace AHStats.services;

public class AuctionService(IDbService dbService) : IAuctionService
{
    public async Task<bool> CreateAuction(Auction auction)
    {
        await dbService.EditData("""
                                    INSERT INTO auction (id) VALUES (@Id)
                                 """, auction);

        return true;
    }

    public async Task<Auction?> GetAuction(string id)
    {
        return await dbService.GetAsync<Auction>("""SELECT * FROM auction WHERE id=@Id""", new { id });
    }

    public async Task<Auction?> GetLatestAuction()
    {
        return await dbService.GetAsync<Auction>("""SELECT * FROM auction ORDER BY created DESC LIMIT 1""", new {});
    }
}