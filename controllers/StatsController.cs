using AHStats.gateways;
using AHStats.services;
using Microsoft.AspNetCore.Mvc;

namespace AHStats.controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController(WowClient wowClient, IItemDataService itemDataService,
    IAuctionService auctionService, IAuctionEntryService auctionEntryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Ping()
    {
        return Ok(await wowClient.GetAuctions());
    }

    [HttpGet("item/{id}")]
    public async Task<IActionResult> GetItem(long id)
    {
        var itemData = await wowClient.GetItemData(id);

        if (itemData == null) return NotFound();

        await itemDataService.CreateItemData(itemData);
        
        return Ok(itemData);
    }

    [HttpGet("sold")]
    public async Task<IActionResult> GetSoldByTime([FromQuery(Name = "hours")]int hours,
        [FromQuery(Name = "name")] string name)
    {
        var items = await itemDataService.GetItemDataFromName(name);
        if (items.Count == 0) return NotFound();

        var auctionIds = (await auctionService.GetAuctionByAge(hours)).Select(a => a.Id).ToList();

        var itemSoldList = new List<object>();
        
        foreach (var item in items)
        {
            var soldEntries = 
                await auctionEntryService.GetSoldAuctionEntriesByActionIdAndItemIds(auctionIds, item.Id);
            soldEntries.Sort((a, b) => 
                a.BuyoutPerItem - b.BuyoutPerItem);
            var toSkip = (int)Math.Floor(soldEntries.Count * 0.05);
            soldEntries = soldEntries.Skip(toSkip).Take(soldEntries.Count - toSkip * 2).ToList();

            if (soldEntries.Count == 0)
            {
                itemSoldList.Add(new
                {
                    msg = "No sold entries for this item",
                    name = item.Name
                });
            }
            else
            {
                var totalSold = soldEntries.Sum(se => se.Quantity);
                var median = 0;
                var count = totalSold;
                
                foreach (var soldEntry in soldEntries)
                {
                    count -= soldEntry.Quantity;
                    if(count > 0) continue;

                    median = soldEntry.BuyoutPerItem;
                    break;
                }
                
                itemSoldList.Add(new
                {
                    avg = soldEntries.Sum(se => se.BuyoutPerItem) / totalSold,
                    median,
                    min = soldEntries[0].BuyoutPerItem,
                    max = soldEntries[^1].BuyoutPerItem,
                    totalSold,
                    name = item.Name
                });
            }
            
        }

        return Ok(itemSoldList);
    }
}