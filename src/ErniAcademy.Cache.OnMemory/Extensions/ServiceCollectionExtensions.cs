using ErniAcademy.Cache.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ErniAcademy.Cache.OnMemory.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Extension method to configure ICacheManager contract with OnMemoryCacheManager impl
    /// </summary>
    /// <param name="services">the ServiceCollection</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddCacheOnMemory(this IServiceCollection services)
    {
        services.TryAddSingleton<ICacheManager, OnMemoryCacheManager>();

        return services;
    }

    /// <summary>
    /// Extension method to configure ICacheManager contract with OnMemoryCacheManager impl
    /// </summary>
    /// <param name="services">the ServiceCollection</param>
    /// <param name="options">the options of the cache</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddCacheOnMemory(this IServiceCollection services, MemoryCacheOptions options)
    {
        services.TryAddSingleton<ICacheManager, OnMemoryCacheManager>();

        return services;
    }
}
