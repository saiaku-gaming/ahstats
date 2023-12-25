using AHStats.gateways.models;

namespace AHStats.services;

public class ItemDataService(IDbService dbService) : IItemDataService
{
    public async Task<bool> CreateItemData(ItemData itemData)
    {
        await dbService.EditData("""
                                  INSERT INTO item_data (
                                     id,
                                     name,
                                     quality,
                                     level,
                                     required_level,
                                     item_class,
                                     item_subclass,
                                     inventory_type,
                                     purchase_price,
                                     sell_price,
                                     max_count,
                                     is_equippable,
                                     is_stackable) VALUES (
                                       @Id,
                                       @Name,
                                       @Quality,
                                       @Level,
                                       @RequiredLevel,
                                       @ItemClass,
                                       @ItemSubclass,
                                       @InventoryType,
                                       @PurchasePrice,
                                       @SellPrice,
                                       @MaxCount,
                                       @IsEquippable,
                                       @IsStackable
                                     )
                                  """, itemData);

        return true;
    }

    public async Task<ItemData?> GetItemData(int id)
    {
        return await dbService.GetAsync<ItemData>("SELECT * FROM item_data WHERE id=@Id", new { id });
    }

    public async Task<List<ItemData>> GetItemDataFromName(string name)
    {
        return await dbService.GetAll<ItemData>("""
            SELECT * FROM item_data WHERE name ilike @Name                                        
        """, new { name = $"%{name}%" });
    }

    public async Task<List<ItemData>> GetItemDataList()
    {
        return await dbService.GetAll<ItemData>("SELECT * FROM item_data", new { });
    }

    public async Task<ItemData> UpdateItemData(ItemData itemData)
    {
        await dbService.EditData("""
                                    UPDATE item_data 
                                    SET 
                                        name=@Name, 
                                        quality=@Quality, 
                                        level=@Level, 
                                        required_level=@RequiredLevel, 
                                        item_class=@ItemClass,
                                        item_subclass=@ItemSubclass,
                                        inventory_type=@InventoryType,
                                        purchase_price=@PurchasePrice,
                                        sell_price=@SellPrice,
                                        max_count=@MaxCount,
                                        is_equippable=@IsEquippable,
                                        is_stackable=@IsStackable
                                  """, itemData);
        return itemData;
    }

    public async Task<bool> DeleteItemData(int id)
    {
        await dbService.EditData("DELETE FROM item_data WHERE id=@Id", new { id });

        return true;
    }
}