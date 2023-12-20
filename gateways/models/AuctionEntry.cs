using AHStats.gateways.models.raw;

namespace AHStats.gateways.models;

public class AuctionEntry
{
    public long Id { get; set; }
    public ItemData? Item { get; set; }
    public int Bid { get; set; }
    public int Buyout { get; set; }
    public int Quantity { get; set; }
    public string TimeLeft { get; set; } = "";

    public static AuctionEntry Map(RawAuction rawAuction)
    {
        return new AuctionEntry
        {
          Id = rawAuction.Id,
          Bid = rawAuction.Bid,
          Buyout = rawAuction.Buyout,
          Quantity = rawAuction.Quantity,
          TimeLeft = rawAuction.time_left
        };
    }
}