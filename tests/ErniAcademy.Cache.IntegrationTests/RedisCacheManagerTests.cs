using ErniAcademy.Cache.Redis.Extensions;
using ErniAcademy.Serializers.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ErniAcademy.Cache.IntegrationTests;

public class RedisCacheManagerTests : BaseTests
{
    protected override IServiceCollection RegisterSut(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCacheRedis(configuration, new JsonSerializer(), "Cache:Redis");
        return services;
    }
}
