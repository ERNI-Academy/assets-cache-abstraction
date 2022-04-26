using ErniAcademy.Cache.StorageBlobs.Extensions;
using ErniAcademy.Serializers.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ErniAcademy.Cache.IntegrationTests;

//[Trait("Cache", " StorageBlobs")]
//public class StorageBlobsCacheManagerTests : BaseTests
//{
//    protected override IServiceCollection RegisterSut(IServiceCollection services, IConfiguration configuration)
//    {
//        services.AddCacheStorageBlobs(configuration, new JsonSerializer(), "Cache:StorageBlobs");
//        return services;
//    }
//}
