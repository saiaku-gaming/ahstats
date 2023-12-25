using AHStats.gateways.models;

namespace AHStats.services;

public class AuctionEntryService(IDbService dbService) : IAuctionEntryService
{
    public async Task<int> UpdateSoldAuctionEntries(string previousAuctionId, List<int> soldAuctionEntries)
    {
        return await dbService.EditData("""
            UPDATE auction_entry SET sold = true WHERE auction_id = @PreviousAuctionId AND id = ANY (@SoldAuctionEntries)
        """, new { previousAuctionId, soldAuctionEntries });
    }

    public async Task<List<AuctionEntry>> GetAuctionEntriesFromAuctionId(string auctionId)
    {
        return await dbService.GetAll<AuctionEntry>("""
                SELECT * FROM auction_entry WHERE auction_id = @AuctionId
        """, new { auctionId });
    }

    public Task CreateAuctionEntries(IEnumerable<AuctionEntry> auctionEntries)
    {
        return dbService.BulkInsert(auctionEntries);
    }

    public async Task<List<AuctionEntry>> GetSoldAuctionEntriesByActionIdAndItemIds(List<string> auctionIds, int itemId)
    {
        return await dbService.GetAll<AuctionEntry>("""
            SELECT * FROM auction_entry 
                WHERE sold AND item_id = @ItemId AND auction_id = ANY (@AuctionIds)
                ORDER BY buyout ASC
        """, new { auctionIds, itemId });
    }
}