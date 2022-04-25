using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.OnMemory.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ErniAcademy.Cache.OnMemory.UnitTests.ServiceCollectionExtensionsTests;

public class AddCacheOnMemory
{
    [Fact]
    public void With_valid_options_section_Should_configure_ICacheManager_with_OnMemory_impl()
    {
        //Arrange
        var services = new ServiceCollection();
        services.AddCacheOnMemory();
        var provider = services.BuildServiceProvider();

        //Act
        var actual = provider.GetRequiredService<ICacheManager>();

        //Assert
        actual.Should().NotBeNull();
    }
}
