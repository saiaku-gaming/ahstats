using AHStats.gateways.models;

namespace AHStats.services;

public class AuctionService(IDbService dbService) : IAuctionService
{
    public async Task<bool> CreateAuction(Auction auction)
    {
        await dbService.EditData("""
                                    INSERT INTO auction (id, auction_house) VALUES (@Id, @AuctionHouse)
                                 """, auction);

        return true;
    }

    public async Task<Auction?> GetAuction(string id)
    {
        return await dbService.GetAsync<Auction>("""SELECT * FROM auction WHERE id=@Id""", new { id });
    }

    public async Task<Auction?> GetLatestAuction(AuctionHouse auctionHouse)
    {
        return await dbService.GetAsync<Auction>("""SELECT * FROM auction WHERE auction_house = @auctionHouse ORDER BY created DESC LIMIT 1""",
            new { AuctionHouse = auctionHouse });
    }

    public async Task<List<Auction>> GetAuctionByAge(int hours, int auctionHouse)
    {
        return await dbService.GetAll<Auction>("""
            SELECT * FROM auction WHERE created >= NOW() - @Interval AND auction_house = @AuctionHouse
        """, new { Interval = TimeSpan.FromHours(hours), AuctionHouse = auctionHouse });
    }

    public async Task<bool> DeleteAuction(string id)
    {
        await dbService.EditData("DELETE FROM auction WHERE id=@Id", new { id });

        return true;
    }
}