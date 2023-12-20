using AHStats.gateways.models;

namespace AHStats.services;

public class AuctionEntryService(IDbService dbService) : IAuctionEntryService
{
    public async Task<bool> CreateAuctionEntry(string auctionId, AuctionEntry auctionEntry)
    {
        await dbService.EditData("""
                                    INSERT INTO auction_entry(id, auction_id, item_id, bid, buyout, quantity, time_left)
                                    VALUES (@Id, @AuctionId, @ItemId, @Bid, @Buyout, @Quantity, @TimeLeft)
                                 """, new { auctionEntry.Id, auctionId, ItemId = auctionEntry.Item.Id, auctionEntry.Bid, auctionEntry.Buyout, auctionEntry.Quantity, auctionEntry.TimeLeft });

        return true;
    }
}