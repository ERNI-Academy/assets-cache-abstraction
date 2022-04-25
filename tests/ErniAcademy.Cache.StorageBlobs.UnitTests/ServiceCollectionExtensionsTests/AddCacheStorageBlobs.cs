using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.StorageBlobs.Extensions;
using ErniAcademy.Serializers.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace ErniAcademy.Cache.StorageBlobs.UnitTests.ServiceCollectionExtensionsTests;

public class AddCacheStorageBlobs
{
    private readonly ISerializer _serializer;

    public AddCacheStorageBlobs()
    {
        _serializer = Substitute.For<ISerializer>();
    }

    [Fact]
    public void With_valid_options_section_Should_configure_ICacheManager_with_StorageBlobs_impl()
    {
        //Arrange
        var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>("StorageBlobs:ConnectionString", "UseDevelopmentStorage=true"),
                }).Build();

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddDebug());
        services.AddCacheStorageBlobs(configuration, _serializer, "StorageBlobs");
        var provider = services.BuildServiceProvider();

        //Act
        var actual = provider.GetRequiredService<ICacheManager>();

        //Assert
        actual.Should().NotBeNull();
    }
}
