using AHStats.gateways.models;

namespace AHStats.services;

public interface IItemDataService
{
    Task<bool> CreateItemData(ItemData itemData);
    Task<ItemData?> GetItemData(int id);
    Task<List<ItemData>> GetItemDataList();
    Task<ItemData> UpdateItemData(ItemData itemData);
    Task<bool> DeleteItemData(int id);
}