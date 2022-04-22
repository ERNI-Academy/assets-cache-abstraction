using ErniAcademy.Cache.OnMemory;
using FluentAssertions;
using System;
using Xunit;

namespace ErniAcademy.Cache.StorageBlobs.UnitTests.OnMemoryCacheManagerTests;

public class GuardValue
{
    [Fact]
    public void With_null_value_Throws_ArgumentNullException()
    {
        //Arrange
        string value = null;

        //Act
        var actual = () => OnMemoryCacheManager.GuardValue(value);

        //Assert
        var error = actual.Should().Throw<ArgumentNullException>();
        error.Which.Message.Should().Contain("value");
    }

    [Theory]
    [InlineData("valid value")]
    [InlineData("also a value")]
    public void With_valid_value_Should_not_throw(string value)
    {
        //Act
        var actual = () => OnMemoryCacheManager.GuardValue(value);

        //Assert
        var error = actual.Should().NotThrow();
    }
}
