using Npgsql;

namespace Project._1.Template._1.Repositories;

public class DatabaseConnectionConfig
{
    public required string? Host { get; set; }

    public required int Port { get; set; }

    public required string? Database { get; set; }

    public required string? SearchPath { get; set; }

    public required string? User { get; set; }

    public required string? Password { get; set; }

    public required SslMode SslMode { get; set; }

    public required bool TrustServerCertificate { get; set; }

    public required int MaxPoolSize { get; set; }

    public required int Timeout { get; set; }
}
