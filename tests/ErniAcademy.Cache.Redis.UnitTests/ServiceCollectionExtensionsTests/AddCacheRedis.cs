using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.Redis.Extensions;
using ErniAcademy.Serializers.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace ErniAcademy.Cache.Redis.UnitTests.ServiceCollectionExtensionsTests;

public class AddCacheRedis
{
    private readonly ISerializer _serializer;

    public AddCacheRedis()
    {
        _serializer = Substitute.For<ISerializer>();
    }

    [Theory]
    [InlineData("Redis:ConnectionString", "")]
    [InlineData("Redis:ConnectionString", "  ")]
    [InlineData("Redis:ConnectionString", null)]
    public void With_wrong_connectionstring_options_section_Throws_OptionsValidationException(string key, string value)
    {
        //Arrange
        var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>(key, value)
                }).Build();

        var services = new ServiceCollection();
        services.AddCacheRedis(configuration, _serializer, "Redis");
        var provider = services.BuildServiceProvider();

        //Act
        var actual = () => provider.GetRequiredService<ICacheManager>();

        //Assert
        var error = actual.Should().Throw<OptionsValidationException>();
        error.Which.Message.Should().Contain("DataAnnotation validation failed for 'ConnectionStringOptions'");
    }
}
