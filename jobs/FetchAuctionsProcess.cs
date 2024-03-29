﻿using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AHStats.gateways;
using AHStats.gateways.models;
using AHStats.services;

namespace AHStats.jobs;

public class FetchAuctionsProcess(WowClient wowClient, IItemDataService itemDataService, 
    IAuctionService auctionService, IAuctionEntryService auctionEntryService, ILogger<FetchAuctionsProcess> logger) : IFetchAuctionsProcess
{
    public async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            string? auctionId = null;

            try
            {
                for (var i = 0; i < 2; ++i)
                {
                    var auctionHouse = i == 0 ? AuctionHouse.Horde : AuctionHouse.Neutral;
                    
                    var auctionsResponse = await wowClient.GetAuctions(auctionHouse);

                    auctionId = CreateMD5(JsonSerializer.Serialize(auctionsResponse));

                    var auction = await auctionService.GetAuction(auctionId);

                    if (auction == null)
                    {
                        var previousAuction = await auctionService.GetLatestAuction(auctionHouse);

                        var timer = new Stopwatch();
                        timer.Start();

                        logger.LogInformation($"New Auction found with id: {auctionId} for house: {auctionHouse}");
                        logger.LogInformation("Fetch started");

                        await auctionService.CreateAuction(new Auction { Id = auctionId, AuctionHouse = auctionHouse });

                        var counter = 1;

                        var newAuctionEntries = auctionsResponse.Auctions
                            .Select(rae => AuctionEntry.Map(auctionId, rae)).ToList();

                        var itemIds = newAuctionEntries.Select(nae => nae.ItemId).Distinct().ToList();

                        foreach (var itemId in itemIds)
                        {
                            var item = await itemDataService.GetItemData(itemId);

                            if (item == null)
                            {
                                item = await wowClient.GetItemData(itemId);
                                ++counter;

                                if (item == null)
                                {
                                    logger.LogWarning($"Unable to find item with id: {itemId}");
                                    newAuctionEntries.RemoveAll(nae => nae.ItemId == itemId);
                                    continue;
                                }

                                await itemDataService.CreateItemData(item);
                            }

                            if (counter != 100) continue;

                            logger.LogInformation("100 requests reached, waiting 2 seconds...");
                            counter = 0;
                            await Task.Delay(2000, stoppingToken);
                        }

                        await auctionEntryService.CreateAuctionEntries(newAuctionEntries);

                        timer.Stop();
                        var timeTaken = timer.Elapsed;

                        logger.LogInformation($"Fetch finished, time elapsed: {timeTaken:m\\:ss\\.fff}");

                        if (previousAuction != null)
                        {
                            logger.LogInformation("Update sold items...");

                            var previousAuctionEntries =
                                await auctionEntryService.GetAuctionEntriesFromAuctionId(previousAuction.Id);

                            var newAuctionEntryIds = newAuctionEntries.Select(nae => nae.Id);

                            previousAuctionEntries.RemoveAll(pae =>
                                pae.Buyout <= 0 || pae.TimeLeft == "SHORT" || pae.TimeLeft == "MEDIUM"
                                || newAuctionEntryIds.Contains(pae.Id));


                            var result = await auctionEntryService.UpdateSoldAuctionEntries(previousAuction.Id,
                                previousAuctionEntries.Select(pae => pae.Id).ToList());

                            logger.LogInformation($"Update finished, {result} items sold");
                        }
                    }
                }
                
                await Task.Delay(1000 * 60 * 10, stoppingToken); 
            }
            catch (Exception e)
            {
                if (auctionId != null)
                {
                    await auctionService.DeleteAuction(auctionId);
                }
                
                logger.LogError(e, "Error accrued while fetching auction data");
                
                await Task.Delay(1000 * 60 * 1, stoppingToken); 
            }
        }
    }

    // ReSharper disable once InconsistentNaming
    private static string CreateMD5(string input)
    {

        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}