using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
#if (metric)
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
#endif
using Serilog;
using Serilog.Events;
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0005
// ReSharper disable once RedundantUsingDirective
using Serilog.Formatting.Compact;
#pragma warning restore IDE0005
#pragma warning restore IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0005
// ReSharper disable once RedundantUsingDirective
using Serilog.Sinks.SystemConsole.Themes;
#pragma warning restore IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning restore IDE0005
#pragma warning restore IDE0079 // Remove unnecessary suppression

namespace Project._1.Template._1;

public class Program
{
    private static void ConfigureDependencyInjection(IServiceCollection services, ConfigurationManager configurationManager)
    {
#if (database)
        ConfigureDatabaseConnection(services, configurationManager);
#endif
#if (metric)
        ConfigureMetrics();
        ConfigureMetricsEndpoint();
#endif

#if (database)
        static void ConfigureDatabaseConnection(IServiceCollection serviceCollection, ConfigurationManager configurationManager)
        {
            serviceCollection.Configure<Repositories.DatabaseConnectionConfig>(configurationManager.GetSection(nameof(Repositories.DatabaseConnectionConfig)));
            serviceCollection.AddSingleton<Repositories.DatabaseConnectionProvider>(); // singleton because the the connection string is only created once
            serviceCollection.AddHostedService<Repositories.DatabaseConfigurationCheck>();
        }
#endif

#if (metric)
        void ConfigureMetrics()
        {
            services.AddSingleton<Operations.Metrics.Metric>();
        }

        void ConfigureMetricsEndpoint()
        {
            services.AddOpenTelemetry()
                .WithMetrics(metricBuilder => metricBuilder
                        .AddMeter(Operations.Metrics.Metric.ApplicationName)
//-:cnd:noEmit
#if DEBUG
//+:cnd:noEmit
                        .AddConsoleExporter(builder => builder.Targets = ConsoleExporterOutputTargets.Console)
//-:cnd:noEmit
#endif
//+:cnd:noEmit
                );
        }
#endif
    }


    public static async Task<int> Main(string[] args)
    {
        try
        {
            SetSerilogDefaultLogger();
            Log.ForContext<Program>().Information("Starting application.");

            var builder = CreateWebApplication();
            var app = builder.Build();

            ConfigureApp(app);

            await app.RunAsync();

            return 0;
        }
        catch (OperationCanceledException)
        {
            return 0;
        }
        catch (Exception ex)
        {
            SetSerilogDefaultLogger();
            Log.ForContext<Program>().Fatal(ex, "Application terminated unexpectedly.");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
            SetSerilogDefaultLogger();
            Log.ForContext<Program>().Information("Application stopped.");
            Log.CloseAndFlush();
        }
    }

    public static WebApplicationBuilder CreateWebApplication()
    {
        var builder = WebApplication.CreateBuilder();
        ConfigureConfiguration(builder.Configuration);
        ConfigureHost(builder.Host);
        ConfigureWebHost(builder.WebHost);
        ConfigureServices(builder.Services, builder.Configuration);
        return builder;
    }

    private static void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        ConfigureDependencyInjection(services, configuration);

        services.AddGrpc();
        services.AddControllers().AddControllersAsServices();

        const string live = "live";
        const string ready = "ready";
        services.AddHealthChecks()
            .AddCheck<Operations.HealthChecks.Diagnostics.HttpHealthCheck>(
                "http_health_check",
                HealthStatus.Unhealthy,
                new[] { ready });
        services.AddGrpcHealthChecks(options =>
            {
                options.Services.MapService(live, _ => false);
                options.Services.MapService(ready, check => check.Tags.Contains(ready));
            })
            .AddCheck<Operations.HealthChecks.Grpc.GrpcHealthCheck>(
                "grpc_health_check",
                HealthStatus.Unhealthy,
                new[] { ready });
    }

    private static void ConfigureConfiguration(IConfigurationBuilder configuration)
    {
        configuration.AddEnvironmentVariables();
        LogConfiguration();

        void LogConfiguration()
        {
            foreach (IConfigurationSource source in configuration.Sources)
            {
                if (source is JsonConfigurationSource jsonConfigurationSource)
                {
                    Log.ForContext<Program>().Debug("Configuration loaded from json file: {jsonConfigurationSource.Path}",
                        jsonConfigurationSource.Path);
                }
                else
                {
                    Log.ForContext<Program>().Debug("Configuration loaded from source type: {source}", source);
                }
            }
        }
    }

    private static void ConfigureApp(WebApplication app)
    {
        app.UseRouting();

        app.MapHealthChecks("/health/live", new HealthCheckOptions() { Predicate = _ => false }); // runs no checks, just to test if application is live
        app.MapHealthChecks("/health/ready", new HealthCheckOptions()); // run all health checks
        app.MapGrpcHealthChecksService();
        app.MapControllers();
    }

    private static void ConfigureHost(ConfigureHostBuilder host)
    {
        host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            string[] excludedRequestPaths =
            {
                "/health"
            };

            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.WithProperty("AssemblyVersion", typeof(Program).Assembly.GetName().Version)
                .Filter.ByExcluding( // removes logs from requests to excluded services
                    logEvent => FilterByRequestPath(logEvent,
                        excludedRequestPaths,
                        LogEventLevel.Warning
                    ));
        });
    }

    private static bool FilterByRequestPath(LogEvent logEvent, string[] requestPathStartsWith, LogEventLevel minimumLevel)
    {
        if (logEvent.Exception == null && logEvent.Level < minimumLevel)
        {
            if (logEvent.Properties.TryGetValue("RequestPath", out LogEventPropertyValue? requestPathValue))
            {
                string? requestPath = (requestPathValue as ScalarValue)?.Value?.ToString();
                if (requestPath != null)
                {
                    foreach (string filter in requestPathStartsWith)
                    {
                        if (requestPath.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private static void ConfigureWebHost(ConfigureWebHostBuilder webHost)
    {
        webHost.ConfigureKestrel((context, serverOptions) =>
        {
            serverOptions.ListenAnyIP(8080, listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });
            serverOptions.ListenAnyIP(3110, listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
            // serverOptions.ListenAnyIP(7106, listenOptions => { listenOptions.Protocols = HttpProtocols.Http3; });
        });
    }

    /// <summary>
    ///     Creates a default logger that is only used until the application configuration was loaded.
    /// </summary>
    private static void SetSerilogDefaultLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("AssemblyVersion", typeof(Program).Assembly.GetName().Version)
//-:cnd:noEmit
#if DEBUG
//+:cnd:noEmit
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:o}] [{Level:u3}] [{Application}] [{Message}] [{Exception}] [{Properties:j}] {NewLine}"
            )
//-:cnd:noEmit
#else
//+:cnd:noEmit
            .WriteTo.Console(new RenderedCompactJsonFormatter())
//-:cnd:noEmit
#endif
//+:cnd:noEmit
            .CreateLogger();
    }
}