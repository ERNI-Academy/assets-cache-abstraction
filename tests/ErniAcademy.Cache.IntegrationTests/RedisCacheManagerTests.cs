using ErniAcademy.Cache.Redis.Extensions;
using ErniAcademy.Serializers.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ErniAcademy.Cache.IntegrationTests;

//[Trait("Cache", " Redis")]
//public class RedisCacheManagerTests : BaseTests
//{
//    protected override IServiceCollection RegisterSut(IServiceCollection services, IConfiguration configuration)
//    {
//        services.AddCacheRedis(configuration, new JsonSerializer(), "Cache:Redis");
//        return services;
//    }
//}
