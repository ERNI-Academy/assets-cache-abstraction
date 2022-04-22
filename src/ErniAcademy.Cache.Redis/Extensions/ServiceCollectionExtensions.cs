using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.Redis.Configuration;
using ErniAcademy.Serializers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ErniAcademy.Cache.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Extension method to configure IEventPublisher contract with RedisPublisher by default will use connection string options 
    /// to connect to Redis database, make sure ConnectionString of Redis is configure in the configuration section.
    /// </summary>
    /// <param name="services">the ServiceCollection</param>
    /// <param name="configuration">the Configuration used to bind and configure the options</param>
    /// <param name="serializer">the serializer to be use</param>
    /// <param name="sectionKey">the configuration section key to get the options</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddEventsPublisherRedis(this IServiceCollection services,
        IConfiguration configuration,
        ISerializer serializer,
        string sectionKey)
    {
        services.AddOptions<ConnectionStringOptions>().Bind(configuration.GetSection(sectionKey)).ValidateDataAnnotations();
        services.AddOptions<RedisCacheOptions>().Bind(configuration.GetSection(sectionKey)).ValidateDataAnnotations();
        
        services.TryAddSingleton<IConnectionMultiplexerProvider, ConnectionMultiplexerProvider>();
        
        services.TryAddSingleton<ICacheManager>(provider =>
        {
            var connectionMultiplexerProvider = provider.GetRequiredService<IConnectionMultiplexerProvider>();

            return new RedisCacheManager(connectionMultiplexerProvider, serializer, provider.GetRequiredService<IOptionsMonitor<RedisCacheOptions>>());
        });

        return services;
    }
}
