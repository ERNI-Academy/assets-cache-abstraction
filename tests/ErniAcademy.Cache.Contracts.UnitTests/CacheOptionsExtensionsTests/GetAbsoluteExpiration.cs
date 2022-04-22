using ErniAcademy.Cache.Contracts.Extensions;
using FluentAssertions;
using System;
using Xunit;

namespace ErniAcademy.Cache.Contracts.UnitTests.CacheOptionsExtensionsTests;

public class GetAbsoluteExpiration
{
    [Fact]
    public void With_AbsoluteExpiration_in_the_past_Throws_ArgumentOutOfRangeException()
    {
        //Arrange
        var creationTime = DateTimeOffset.UtcNow;
        var cacheOptions = new CacheOptions();
        cacheOptions.AbsoluteExpiration = creationTime.AddSeconds(-1);

        //Act
        var actual = () => cacheOptions.GetAbsoluteExpiration(creationTime);

        //Assert
        var error = actual.Should().Throw<ArgumentOutOfRangeException>();
        error.Which.Message.Should().Contain("The AbsoluteExpiration value must be in the future.");
    }

    [Fact]
    public void With_AbsoluteExpirationRelativeToNow_with_value_Should_use_it_plus_creation_time()
    {
        //Arrange
        var creationTime = DateTimeOffset.UtcNow;
        var cacheOptions = new CacheOptions();
        cacheOptions.AbsoluteExpiration = creationTime.AddSeconds(1);
        cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

        //Act
        var actual = cacheOptions.GetAbsoluteExpiration(creationTime);

        //Assert
        actual.Value.Should().Be(creationTime + TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void With_AbsoluteExpirationRelativeToNow_without_value_Should_use_absoluteExpiration()
    {
        //Arrange
        var creationTime = DateTimeOffset.UtcNow;
        var cacheOptions = new CacheOptions();
        cacheOptions.AbsoluteExpiration = creationTime.AddSeconds(1);

        //Act
        var actual = cacheOptions.GetAbsoluteExpiration(creationTime);

        //Assert
        actual.Value.Should().Be(cacheOptions.AbsoluteExpiration);
    }
}
