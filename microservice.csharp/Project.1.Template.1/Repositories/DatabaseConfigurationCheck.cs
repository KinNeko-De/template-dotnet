using System.Diagnostics;
#if (metric)
using Project._1.Template._1.Operations.Metrics;
#endif

namespace Project._1.Template._1.Repositories;

/// <summary>
/// Checks if the configuration of the database is correct and a connection can be established.
/// After that the background service ends.
/// </summary>
public class DatabaseConfigurationCheck : BackgroundService
{
    private readonly ILogger<DatabaseConfigurationCheck> logger;
#if (metric)
    private readonly Metric metric;
#endif
    private readonly DatabaseConnectionProvider databaseConnectionProvider;

    public DatabaseConfigurationCheck(
        ILogger<DatabaseConfigurationCheck> logger,
#if (metric)
        Metric metric,
#endif
        DatabaseConnectionProvider databaseConnectionProvider
        )
    {
        this.logger = logger;
#if (metric)
        this.metric = metric;
#endif
        this.databaseConnectionProvider = databaseConnectionProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Yield(); // ensures that the background service is not blocking the start up of the service

        while (!cancellationToken.IsCancellationRequested)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                await using var connection = await databaseConnectionProvider.GetOpenConnection(cancellationToken);

                stopwatch.Stop();
                logger.LogInformation("DatabaseConnection could be established. Needed {ElapsedMilliseconds} ms.", stopwatch.ElapsedMilliseconds);
                break;
            }
            catch (Exception exception)
            {
                stopwatch.Stop();
                logger.LogError(exception, "DatabaseConnection can not be established. Duration {ElapsedMilliseconds} ms.", stopwatch.ElapsedMilliseconds);
#if (metric)
                metric.DatabaseConnectionErrorOccurred();
#endif
            }
        }

        logger.LogInformation("Shutting downing after connection succeed.");
    }

}
