namespace AHStats.gateways.models.raw;

public class RawAuction
{
    public long Id { get; set; }
    public RawAuctionItem Item { get; set; }
    public int Bid { get; set; }
    public int Buyout { get; set; }
    public int Quantity { get; set; }
    public string time_left { get; set; } = "";
}