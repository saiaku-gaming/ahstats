namespace AHStats.jobs;

public class FetchAuctionsJob(IServiceProvider services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();
        
        var fetchAuctionsProcess = scope.ServiceProvider.GetRequiredService<IFetchAuctionsProcess>();

        await fetchAuctionsProcess.DoWork(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
    }
}