namespace AHStats.jobs;

public interface IFetchAuctionsProcess
{
    Task DoWork(CancellationToken stoppingToken);
}