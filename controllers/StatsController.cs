using AHStats.gateways;
using AHStats.services;
using Microsoft.AspNetCore.Mvc;

namespace AHStats.controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController(WowClient wowClient, IItemDataService itemDataService) : ControllerBase
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
}