using System.Diagnostics.Metrics;

namespace Project._1.Template._1.Operations.Metrics;

public class Metric
{
    public const string ApplicationName = "project.1-domain.1-type.1";

    private static readonly Meter Meter = new (ApplicationName, "1.0.0");

#if (database)
    private static readonly Counter<int> DatabaseConnectionError = Meter.CreateCounter<int>("database-connection-error", "error", "Connection errors to the database");

    public void DatabaseConnectionErrorOccurred()
    {
        DatabaseConnectionError.Add(1);
    }
#endif
}
