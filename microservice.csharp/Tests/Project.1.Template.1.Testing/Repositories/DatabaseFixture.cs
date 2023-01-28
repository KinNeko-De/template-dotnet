using Microsoft.Extensions.Options;
using Npgsql;
using Project._1.Template._1.Repositories;

namespace Project._1.Template._1.Testing.Repositories;
public class DatabaseFixture
{
    private static DatabaseConnectionConfig ConnectToLocalDatabase()
    {
        return new DatabaseConnectionConfig()
        {
            Host = "localhost",
            Port = port.database.1,
            Database = "domain.1database",
            User = "domain.1user",
            Password = "domain.1password",
            MaxPoolSize = 5,
            SearchPath = "domain.1",
            SslMode = SslMode.Prefer,
            TrustServerCertificate = true,
            Timeout = 30,
        };
    }

    /// <summary>
    /// For automated tests this connects you to your local running database
    /// </summary>
    /// <returns>DatabaseConnectionProvider to connect to local running test database</returns>
    public static DatabaseConnectionProvider GetConnectionProviderToLocalDatabase()
    {
        IOptions<DatabaseConnectionConfig> databaseConnectionConfig = Options.Create(ConnectToLocalDatabase());
        var databaseConnectionProvider = new DatabaseConnectionProvider(databaseConnectionConfig);
        return databaseConnectionProvider;
    }
}

