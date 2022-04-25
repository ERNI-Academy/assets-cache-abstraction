using FluentAssertions;
using System;
using Xunit;

namespace ErniAcademy.Cache.Contracts.UnitTests.CacheOptionsTests;

public class SlidingExpiration
{
    [Fact]
    public void With_negative_value_Throws_ArgumentOutOfRangeException()
    {
        //Arrange
        var cacheOptions = new CacheOptions();

        //Act
        var actual = ()=> cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(-1);

        //Assert
        var error = actual.Should().Throw<ArgumentOutOfRangeException>();
        error.Which.Message.Should().Contain("The sliding expiration value must be positive.");
    }

    [Fact]
    public void With_positive_value_Should_set()
    {
        //Arrange
        var cacheOptions = new CacheOptions();

        //Act
        var actual = () => cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(1);

        //Assert
        actual.Should().NotThrow();
    }
}
