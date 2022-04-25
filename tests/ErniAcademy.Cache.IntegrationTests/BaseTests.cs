using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.IntegrationTests.Utils;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ErniAcademy.Cache.IntegrationTests;

public abstract class BaseTests
{
    protected ICacheManager _sut;
    protected IServiceProvider _provider;

    protected BaseTests()
    {
        var services = new ServiceCollection();

        var configuration = ConfigurationHelper.Get();

        services.AddSingleton<IConfiguration>(configuration);

        RegisterSut(services, configuration);

        _provider = services.BuildServiceProvider();

        _sut = _provider.GetService<ICacheManager>();
    }

    protected abstract IServiceCollection RegisterSut(IServiceCollection services, IConfiguration configuration);

    [Fact]
    public void Get_with_no_item_in_cache_Returns_default_of_item()
    {
        //Arrange
        var key = "get_with_no_item";

        //Act
        var actual = _sut.Get<CacheItemDummy>(key);

        //Assert
        actual.Should().Be(default(CacheItemDummy));
    }

    [Fact]
    public void Get_with_item_in_cache_Returns_item()
    {
        //Arrange
        var key = "get_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, item);

        //Act
        var actual = _sut.Get<CacheItemDummy>(key);

        //Assert
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public void Get_with_expired_item_in_cache_Returns_default_of_item()
    {
        //Arrange
        var key = "get_with_expired_item";
        var item = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, item, new CacheOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(100) });

        Task.Delay(101).GetAwaiter().GetResult();

        //Act
        var actual = _sut.Get<CacheItemDummy>(key);

        //Assert
        actual.Should().Be(default(CacheItemDummy));
    }

    [Fact]
    public void GetOrAdd_with_no_item_in_cache_Returns_item_after_invoke_factory()
    {
        //Arrange
        var key = "getoradd_with_no_item";
        var item = new CacheItemDummy { Name = "hi" };

        Func<CacheItemDummy> factory = () => item;

        //Act
        var actual = _sut.GetOrAdd<CacheItemDummy>(key, factory);

        //Assert
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public void GetOrAdd_with_item_in_cache_Returns_item_from_cache()
    {
        //Arrange
        var key = "getoradd_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, item);

        Func<CacheItemDummy> factory = () => new CacheItemDummy { Name = "hello" };

        //Act
        var actual = _sut.GetOrAdd<CacheItemDummy>(key, factory);

        //Assert
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task GetAsync_with_no_item_in_cache_Returns_default_of_item()
    {
        //Arrange
        var key = "getasync_with_no_item";

        //Act
        var actual = await _sut.GetAsync<CacheItemDummy>(key);

        //Assert
        actual.Should().Be(default(CacheItemDummy));
    }

    [Fact]
    public async Task GetAsync_with_item_in_cache_Returns_item()
    {
        //Arrange
        var key = "getasync_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        await _sut.SetAsync<CacheItemDummy>(key, item);

        //Act
        var actual = await _sut.GetAsync<CacheItemDummy>(key);

        //Assert
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task GetAsync_with_expired_item_in_cache_Returns_default_of_item()
    {
        //Arrange
        var key = "getasync_with_expired_item";
        var item = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, item, new CacheOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(100) });

        await Task.Delay(101);

        //Act
        var actual = await _sut.GetAsync<CacheItemDummy>(key);

        //Assert
        actual.Should().Be(default(CacheItemDummy));
    }

    [Fact]
    public async Task GetOrAddAsync_with_no_item_in_cache_Returns_item_after_invoke_factory()
    {
        //Arrange
        var key = "getoraddasync_with_no_item";
        var item = new CacheItemDummy { Name = "hi" };

        Func<Task<CacheItemDummy>> factory = () => Task.FromResult(item);

        //Act
        var actual = await _sut.GetOrAddAsync<CacheItemDummy>(key, factory);

        //Assert
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task GetOrAddAsync_with_item_in_cache_Returns_item_from_cache()
    {
        //Arrange
        var key = "getoraddasync_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        await _sut.SetAsync<CacheItemDummy>(key, item);

        Func<Task<CacheItemDummy>> factory = () => Task.FromResult(new CacheItemDummy { Name = "hello" });

        //Act
        var actual = await _sut.GetOrAddAsync<CacheItemDummy>(key, factory);

        //Assert
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public void Set_with_no_item_in_cache_Should_set_item()
    {
        //Arrange
        var key = "set_with_no_item";
        var item = new CacheItemDummy { Name = "hi" };

        //Act
        _sut.Set<CacheItemDummy>(key, item);

        //Assert
        var actual = _sut.Get<CacheItemDummy>(key);
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public void Set_with_item_already_in_cache_Should_update_item()
    {
        //Arrange
        var key = "set_with_item_already";
        var original = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, original);

        var updated = new CacheItemDummy { Name = "hi updated" };

        //Act
        _sut.Set<CacheItemDummy>(key, updated);

        //Assert
        var actual = _sut.Get<CacheItemDummy>(key);
        actual.Should().BeEquivalentTo(updated);
    }

    [Fact]
    public async Task SetAsync_with_no_item_in_cache_Should_set_item()
    {
        //Arrange
        var key = "setasync_with_no_item";
        var item = new CacheItemDummy { Name = "hi" };

        //Act
        await _sut.SetAsync<CacheItemDummy>(key, item);

        //Assert
        var actual = await _sut.GetAsync<CacheItemDummy>(key);
        actual.Should().BeEquivalentTo(item);
    }

    [Fact]
    public async Task SetAsync_with_item_already_in_cache_Should_update_item()
    {
        //Arrange
        var key = "setasync_with_item_already";
        var original = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, original);

        var updated = new CacheItemDummy { Name = "hi updated" };

        //Act
        await _sut.SetAsync<CacheItemDummy>(key, updated);

        //Assert
        var actual = await _sut.GetAsync<CacheItemDummy>(key);
        actual.Should().BeEquivalentTo(updated);
    }

    [Fact]
    public void Exists_with_no_item_in_cache_Returns_false()
    {
        //Arrange
        var key = "exists_with_no_item";

        //Act
        var actual = _sut.Exists(key);

        //Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void Exists_with_item_in_cache_Returns_true()
    {
        //Arrange
        var key = "exists_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, item);

        //Act
        var actual = _sut.Exists(key);

        //Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_with_no_item_in_cache_Returns_false()
    {
        //Arrange
        var key = "existsasync_with_no_item";

        //Act
        var actual = await _sut.ExistsAsync(key);

        //Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsAsync_with_item_in_cache_Returns_true()
    {
        //Arrange
        var key = "existsasync_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        await _sut.SetAsync<CacheItemDummy>(key, item);

        //Act
        var actual = await _sut.ExistsAsync(key);

        //Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void Remove_with_no_item_in_cache_Should_do_nothing()
    {
        //Arrange
        var key = "remove_with_no_item";

        //Act
        var actual = ()=> _sut.Remove(key);

        //Assert
        actual.Should().NotThrow();
    }

    [Fact]
    public void Remove_with_item_in_cache_Should_remove()
    {
        //Arrange
        var key = "remove_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, item);

        //Act
        _sut.Remove(key);

        //Assert
        var actual = _sut.Exists(key);
        actual.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAsync_with_no_item_in_cache_Should_do_nothing()
    {
        //Arrange
        var key = "removeasync_with_no_item";

        //Act
        var actual = ()=> _sut.RemoveAsync(key);

        //Assert
        await actual.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RemoveAsync_with_item_in_cache_Should_remove_item()
    {
        //Arrange
        var key = "removeasync_with_item";
        var item = new CacheItemDummy { Name = "hi" };

        _sut.Set<CacheItemDummy>(key, item);

        //Act
        await _sut.RemoveAsync(key);

        //Assert
        var actual = _sut.Exists(key);
        actual.Should().BeFalse();
    }
}
