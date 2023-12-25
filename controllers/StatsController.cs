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

            if (soldEntries.Count == 0)
            {
                itemSoldList.Add(new
                {
                    avg = -1,
                    min = -1,
                    max = -1,
                    name = item.Name
                });
            }
            else
            {
                itemSoldList.Add(new
                {
                    avg = soldEntries.Sum(se => se.Buyout) / soldEntries.Count,
                    min = soldEntries.Min(se => se.Buyout),
                    max = soldEntries.Max(se => se.Buyout),
                    name = item.Name
                });
            }
            
        }

        return Ok(itemSoldList);
    }
}