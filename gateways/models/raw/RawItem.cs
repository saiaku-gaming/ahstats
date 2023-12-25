namespace AHStats.gateways.models.raw;

public class RawItem
{
    public int Id { get; set; }
    public RawName Name { get; set; }
    public RawQuality Quality { get; set; }
    public int Level { get; set; }
    public int required_level { get; set; }
    public RawItemClass item_class { get; set; }
    public RawSubClass item_subclass { get; set; }
    public RawInventoryType inventory_type { get; set; }
    public int purchase_price { get; set; }
    public int sell_price { get; set; }
    public int max_count { get; set; }
    public bool is_equippable { get; set; }
    public bool is_stackable { get; set; }
}