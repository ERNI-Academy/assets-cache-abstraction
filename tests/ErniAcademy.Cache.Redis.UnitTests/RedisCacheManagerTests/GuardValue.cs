using FluentAssertions;
using System;
using Xunit;

namespace ErniAcademy.Cache.Redis.UnitTests.RedisCacheManagerTests;

public class GuardValue
{
    [Fact]
    public void With_null_value_Throws_ArgumentException()
    {
        //Arrange
        string value = null;

        //Act
        var actual = () => RedisCacheManager.GuardValue(value);

        //Assert
        var error = actual.Should().Throw<ArgumentException>();
        error.Which.Message.Should().Contain("cache a default value is not allowed");
    }

    [Theory]
    [InlineData("valid value")]
    [InlineData('c')]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(5)]
    [InlineData(11.99)]
    public void With_valid_value_Should_not_throw(object value)
    {
        //Act
        var actual = () => RedisCacheManager.GuardValue(value);

        //Assert
        var error = actual.Should().NotThrow();
    }
}
