using FluentAssertions;
using System;
using Xunit;

namespace ErniAcademy.Cache.StorageBlobs.UnitTests.StorageBlobsCacheManagerTests;

public class GuardKey
{
    [Fact]
    public void With_null_key_Throws_ArgumentNullException()
    {
        //Arrange
        string key = null;

        //Act
        var actual = () => StorageBlobsCacheManager.GuardKey(key);

        //Assert
        var error = actual.Should().Throw<ArgumentNullException>();
        error.Which.Message.Should().Contain("key");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void With_invalid_key_Throws_ArgumentException(string key)
    {
        //Act
        var actual = () => StorageBlobsCacheManager.GuardKey(key);

        //Assert
        var error = actual.Should().Throw<ArgumentException>();
        error.Which.Message.Should().Contain("invalid key");
    }

    [Theory]
    [InlineData("valid/key")]
    [InlineData("alsovalid")]
    public void With_valid_key_Should_not_throw(string key)
    {
        //Act
        var actual = () => StorageBlobsCacheManager.GuardKey(key);

        //Assert
        var error = actual.Should().NotThrow();
    }
}
