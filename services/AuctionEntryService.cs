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

    public async Task<int> UpdateSoldAuctionEntries(string previousAuctionId, string currentAuctionId)
    {
        return await dbService.EditData("""
                                        
                                        WITH previous_auction_entries AS (
                                            SELECT id FROM auction_entry WHERE auction_id = @PreviousAuctionId AND time_left IN ('LONG', 'VERY_LONG')
                                        ), current_auction_entries AS (
                                            SELECT id FROM auction_entry WHERE auction_id = @CurrentAuctionId
                                        ), sold_auction_entries AS (
                                            SELECT pae.id FROM previous_auction_entries AS pae LEFT JOIN current_auction_entries AS cae ON(cae.id = pae.id) WHERE cae.id IS NULL
                                        )
                                        UPDATE auction_entry SET sold=true WHERE auction_id = @PreviousAuctionId AND id IN (SELECT id FROM sold_auction_entries)
                                                                                
                                        """, new { previousAuctionId, currentAuctionId });
    }
}