using MSDisEventApplication.Data;
using MSDisEventApplication.Data.Interfaces;
using MSDisEventApplication.Models;
using MSDisEventApplication.Options;
using MSDisEventApplication.Services;
using MSDisEventApplication.Services.Interfaces;

namespace MSDisEventApplication.Extensions;

public static class EventApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services = ConfigureSettings(services, configuration);

        services.AddSingleton<IEventObservable<UserEvent>, EventObservable>();
        services.AddSingleton<IEventObserver, EventObserver>();
        services.AddSingleton<IDataStorage, DbDataStorage>();
        services.AddHostedService<EventBackgroundService>();
        services.AddHostedService<KafkaBackgroundService>();

        return services;
    }

    private static IServiceCollection ConfigureSettings(IServiceCollection services, IConfiguration configuration)
    {
        var useAppsettings = configuration.GetValue<bool>("UseAppsettings");
        services.Configure<KafkaOptions>(options =>
        {
            if (useAppsettings)
            {
                options.BootstrapServers = configuration["Kafka:BootstrapServers"]!;
                options.Topic = configuration["Kafka:Topic"]!;
                options.GroupId = configuration["Kafka:GroupId"]!;
            }
            else
            {
                options.BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS")!;
                options.Topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC")!;
                options.GroupId = Environment.GetEnvironmentVariable("KAFKA_GROUP_ID")!;
            }
        });

        services.Configure<DbOptions>(options =>
        {
            if (useAppsettings)
                options.ConnectionString = configuration["Db:ConnectionString"]!;
            else
                options.ConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")!;
        });

        return services;
    }
}
