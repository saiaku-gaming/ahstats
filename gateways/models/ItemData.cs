using AHStats.gateways.models.raw;

namespace AHStats.gateways.models;

public class ItemData
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Quality { get; set; }
    public int Level { get; set; }
    public int RequiredLevel { get; set; }
    public string ItemClass { get; set; }
    public string ItemSubclass { get; set; }
    public string InventoryType { get; set; }
    public int PurchasePrice { get; set; }
    public int SellPrice { get; set; }
    public int MaxCount { get; set; }
    public bool IsEquippable { get; set; }
    public bool IsStackable { get; set; }

    public static ItemData Map(RawItem rawItem)
    {
        return new ItemData
        {
            Id = rawItem.Id,
            Name = rawItem.Name.en_US,
            IsEquippable = rawItem.is_equippable,
            Level = rawItem.Level,
            Quality = rawItem.Quality.Type,
            InventoryType = rawItem.inventory_type.Type,
            IsStackable = rawItem.is_stackable,
            ItemClass = rawItem.item_class.Name.en_US,
            ItemSubclass = rawItem.item_subclass.Name.en_US,
            MaxCount = rawItem.max_count,
            PurchasePrice = rawItem.purchase_price,
            RequiredLevel = rawItem.required_level,
            SellPrice = rawItem.sell_price
        };
    }
}