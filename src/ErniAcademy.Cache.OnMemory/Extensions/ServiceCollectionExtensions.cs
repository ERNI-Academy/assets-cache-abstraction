using ErniAcademy.Cache.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace ErniAcademy.Cache.OnMemory.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Extension method to configure ICacheManager contract with OnMemoryCacheManager impl
    /// </summary>
    /// <param name="services">the ServiceCollection</param>
    /// <param name="options">the options of the cache</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddCacheOnMemory(this IServiceCollection services, MemoryCacheOptions options = null)
    {
        services.TryAddSingleton<ICacheManager>(provider => {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            return options == null ? new OnMemoryCacheManager(loggerFactory) : new OnMemoryCacheManager(options, loggerFactory);
        });

        return services;
    }
}
