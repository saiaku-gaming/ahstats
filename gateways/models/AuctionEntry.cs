using AHStats.gateways.models.raw;

namespace AHStats.gateways.models;

public class AuctionEntry
{
    public int Id { get; set; }
    public string AuctionId { get; set; }
    public int ItemId { get; set; }
    public int Bid { get; set; }
    public int Buyout { get; set; }
    public int Quantity { get; set; }
    public string TimeLeft { get; set; } = "";

    public static AuctionEntry Map(string auctionId, RawAuction rawAuction)
    {
        return new AuctionEntry
        {
          Id = rawAuction.Id,
          AuctionId = auctionId,
          Bid = rawAuction.Bid,
          Buyout = rawAuction.Buyout,
          Quantity = rawAuction.Quantity,
          TimeLeft = rawAuction.time_left,
          ItemId = rawAuction.Item.Id
        };
    }
}