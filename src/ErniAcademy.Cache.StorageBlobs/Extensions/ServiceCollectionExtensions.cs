using Azure.Storage.Blobs;
using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.StorageBlobs.ClientProvider;
using ErniAcademy.Cache.StorageBlobs.Configuration;
using ErniAcademy.Serializers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ErniAcademy.Cache.StorageBlobs.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Extension method to configure ICacheManager contract with StorageBlobsCacheManager impl
    /// </summary>
    /// <param name="services">the ServiceCollection</param>
    /// <param name="configuration">the Configuration used to bind and configure the options</param>
    /// <param name="serializer">the serializer to be use</param>
    /// <param name="sectionKey">the configuration section key to get the options</param>
    /// <param name="blobOptions">Optional client options that define the transport pipeline policies for authentication, retries, etc., that are applied to every request.</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddCacheStorageBlobs(this IServiceCollection services,
        IConfiguration configuration, 
        ISerializer serializer,
        string sectionKey,
        BlobClientOptions blobOptions = null)
    {
        services.AddOptions<ConnectionStringOptions>().Bind(configuration.GetSection(sectionKey)).ValidateDataAnnotations();
        services.AddOptions<StorageBlobsCacheOptions>().Bind(configuration.GetSection(sectionKey)).ValidateDataAnnotations();

        services.TryAddSingleton<ICacheManager>(provider =>
        {
            var blobContainerClientProvider = new ConnectionStringProvider(provider.GetRequiredService<IOptionsMonitor<ConnectionStringOptions>>(), blobOptions);
            
            return new StorageBlobsCacheManager(blobContainerClientProvider, serializer, provider.GetRequiredService<IOptionsMonitor<StorageBlobsCacheOptions>>());
        });

        return services;
    }
}
