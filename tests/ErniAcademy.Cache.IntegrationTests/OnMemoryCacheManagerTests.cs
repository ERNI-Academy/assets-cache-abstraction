using ErniAcademy.Cache.OnMemory.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ErniAcademy.Cache.IntegrationTests;

[Trait("Cache", "OnMemory")]
public class OnMemoryCacheManagerTests : BaseTests
{
    protected override IServiceCollection RegisterSut(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCacheOnMemory();
        return services;
    }
}
