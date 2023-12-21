using AHStats.extensions;
using AHStats.gateways;
using AHStats.gateways.auth;
using AHStats.jobs;
using AHStats.options;
using AHStats.services;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.Configure<WowClientOptions>(builder.Configuration.GetSection(WowClientOptions.WowClient));

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<TokenAuthHeaderHandler>();
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IItemDataService, ItemDataService>();
builder.Services.AddScoped<IAuctionEntryService, AuctionEntryService>();
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IFetchAuctionsProcess, FetchAuctionsProcess>();
builder.Services.AddHostedService<FetchAuctionsJob>();

builder.Services.AddHttpClient("BNetOAuth", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://oauth.battle.net/");
});

builder.Services.AddHttpClient("WowApi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://eu.api.blizzard.com/");
}).AddHttpMessageHandler<TokenAuthHeaderHandler>();

builder.Services.AddSingleton<WowClient>();

DefaultTypeMap.MatchNamesWithUnderscores = true;

var app = builder.Build();

app.MigrateDatabase<Program>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
