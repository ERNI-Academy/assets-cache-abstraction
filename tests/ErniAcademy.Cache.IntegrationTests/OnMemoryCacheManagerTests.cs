using ErniAcademy.Cache.OnMemory.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ErniAcademy.Cache.IntegrationTests;

public class OnMemoryCacheManagerTests : BaseTests
{
    protected override IServiceCollection RegisterSut(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCacheOnMemory();
        return services;
    }
}
