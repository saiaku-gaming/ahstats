﻿using System.Security.Cryptography;
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
            var auctionsResponse = await wowClient.GetAuctions();

            var auctionId = CreateMD5(JsonSerializer.Serialize(auctionsResponse));

            var auction = await auctionService.GetAuction(auctionId);

            if (auction == null)
            {
                logger.LogInformation($"New Auction found with id: {auctionId}");
                logger.LogInformation("Fetch started");
                
                await auctionService.CreateAuction(new Auction { Id = auctionId });

                var counter = 1;
                
                foreach (var rawAuctionEntry in auctionsResponse.Auctions)
                {
                    var item = await itemDataService.GetItemData(rawAuctionEntry.Item.Id);

                    if (item == null)
                    {
                        item = await wowClient.GetItemData(rawAuctionEntry.Item.Id);
                        ++counter;

                        if (item == null)
                        {
                            logger.LogWarning($"Unable to find item with id: {rawAuctionEntry.Item.Id}");
                            continue;
                        }

                        await itemDataService.CreateItemData(item);
                    }

                    var auctionEntry = AuctionEntry.Map(rawAuctionEntry);
                    auctionEntry.Item = item;

                    await auctionEntryService.CreateAuctionEntry(auctionId, auctionEntry);

                    if (counter != 100) continue;
                    
                    logger.LogInformation("100 requests reached, waiting 2 seconds...");
                    counter = 0;
                    await Task.Delay(2000, stoppingToken);
                }
                
                logger.LogInformation("Fetch finished");
            }
            
            await Task.Delay(1000 * 60 * 10, stoppingToken);
        }
    }

    // ReSharper disable once InconsistentNaming
    private static string CreateMD5(string input)
    {
        using var md5 = MD5.Create();

        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}