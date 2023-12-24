using AHStats.gateways.models;

namespace AHStats.services;

public class AuctionEntryService(IDbService dbService) : IAuctionEntryService
{
    public async Task<bool> CreateAuctionEntry(string auctionId, AuctionEntry auctionEntry)
    {
        await dbService.EditData("""
                                    INSERT INTO auction_entry(id, auction_id, item_id, bid, buyout, quantity, time_left)
                                    VALUES (@Id, @AuctionId, @ItemId, @Bid, @Buyout, @Quantity, @TimeLeft)
                                 """, new { auctionEntry.Id, auctionId, auctionEntry.ItemId, auctionEntry.Bid, auctionEntry.Buyout, auctionEntry.Quantity, auctionEntry.TimeLeft });

        return true;
    }

    public async Task<int> UpdateSoldAuctionEntries(string previousAuctionId, List<int> soldAuctionEntries)
    {
        return await dbService.EditData("""
            UPDATE auction_entry SET sold = true WHERE auction_id = @PreviousAuctionId AND id ANY (@SoldAuctionEntries)
        """, new { previousAuctionId, soldAuctionEntries });
    }

    public async Task<List<AuctionEntry>> GetAuctionEntriesFromAuctionId(string auctionId)
    {
        return await dbService.GetAll<AuctionEntry>("""
                SELECT * FROM auction_entry WHERE auction_id = @AuctionId
        """, new { auctionId });
    }
}