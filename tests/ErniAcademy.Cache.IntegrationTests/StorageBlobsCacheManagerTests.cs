using ErniAcademy.Cache.StorageBlobs.Extensions;
using ErniAcademy.Serializers.Json;
using System.Threading.Tasks;
using Xunit;

namespace ErniAcademy.Cache.IntegrationTests;

[Trait("Cache", " StorageBlobs")]
public class StorageBlobsCacheManagerTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;

    public StorageBlobsCacheManagerTests(TestFixture fixture)
    {
        _fixture = fixture;
        _fixture.Initialize((s, c) => { s.AddCacheStorageBlobs(c, new JsonSerializer(), "Cache:StorageBlobs"); });
    }

    [Fact]
    [Trait("Category", "Get")]
    public void Get_with_no_item_in_cache_Returns_default_of_item()
    {
        _fixture.Get_with_no_item_in_cache_Returns_default_of_item();
    }

    [Fact]
    [Trait("Category", "Get")]
    public void Get_with_item_in_cache_Returns_item()
    {
        _fixture.Get_with_item_in_cache_Returns_item();
    }

    [Fact]
    [Trait("Category", "Get")]
    public void Get_with_expired_item_in_cache_Returns_default_of_item()
    {
        _fixture.Get_with_expired_item_in_cache_Returns_default_of_item();
    }

    [Fact]
    [Trait("Category", "GetOrAdd")]
    public void GetOrAdd_with_no_item_in_cache_Returns_item_after_invoke_factory()
    {
        _fixture.GetOrAdd_with_no_item_in_cache_Returns_item_after_invoke_factory();
    }

    [Fact]
    [Trait("Category", "GetOrAdd")]
    public void GetOrAdd_with_item_in_cache_Returns_item_from_cache()
    {
        _fixture.GetOrAdd_with_item_in_cache_Returns_item_from_cache();
    }

    [Fact]
    [Trait("Category", "GetAsync")]
    public async Task GetAsync_with_no_item_in_cache_Returns_default_of_item()
    {
        await _fixture.GetAsync_with_no_item_in_cache_Returns_default_of_item();
    }

    [Fact]
    [Trait("Category", "GetAsync")]
    public async Task GetAsync_with_item_in_cache_Returns_item()
    {
        await _fixture.GetAsync_with_item_in_cache_Returns_item();
    }

    [Fact]
    [Trait("Category", "GetAsync")]
    public async Task GetAsync_with_expired_item_in_cache_Returns_default_of_item()
    {
        await _fixture.GetAsync_with_expired_item_in_cache_Returns_default_of_item();
    }

    [Fact]
    [Trait("Category", "GetOrAddAsync")]
    public async Task GetOrAddAsync_with_no_item_in_cache_Returns_item_after_invoke_factory()
    {
        await _fixture.GetOrAddAsync_with_no_item_in_cache_Returns_item_after_invoke_factory();
    }

    [Fact]
    [Trait("Category", "GetOrAddAsync")]
    public async Task GetOrAddAsync_with_item_in_cache_Returns_item_from_cache()
    {
        await _fixture.GetOrAddAsync_with_item_in_cache_Returns_item_from_cache();
    }

    [Fact]
    [Trait("Category", "Set")]
    public void Set_with_no_item_in_cache_Should_set_item()
    {
        _fixture.Set_with_no_item_in_cache_Should_set_item();
    }

    [Fact]
    [Trait("Category", "Set")]
    public void Set_with_item_already_in_cache_Should_update_item()
    {
        _fixture.Set_with_item_already_in_cache_Should_update_item();
    }

    [Fact]
    [Trait("Category", "SetAsync")]
    public async Task SetAsync_with_no_item_in_cache_Should_set_item()
    {
        await _fixture.SetAsync_with_no_item_in_cache_Should_set_item();
    }

    [Fact]
    [Trait("Category", "SetAsync")]
    public async Task SetAsync_with_item_already_in_cache_Should_update_item()
    {
        await _fixture.SetAsync_with_item_already_in_cache_Should_update_item();
    }

    [Fact]
    [Trait("Category", "Exists")]
    public void Exists_with_no_item_in_cache_Returns_false()
    {
        _fixture.Exists_with_no_item_in_cache_Returns_false();
    }

    [Fact]
    [Trait("Category", "Exists")]
    public void Exists_with_item_in_cache_Returns_true()
    {
        _fixture.Exists_with_item_in_cache_Returns_true();
    }

    [Fact]
    [Trait("Category", "ExistsAsync")]
    public async Task ExistsAsync_with_no_item_in_cache_Returns_false()
    {
        await _fixture.ExistsAsync_with_no_item_in_cache_Returns_false();
    }

    [Fact]
    [Trait("Category", "ExistsAsync")]
    public async Task ExistsAsync_with_item_in_cache_Returns_true()
    {
        await _fixture.ExistsAsync_with_item_in_cache_Returns_true();
    }

    [Fact]
    [Trait("Category", "Remove")]
    public void Remove_with_no_item_in_cache_Should_do_nothing()
    {
        _fixture.Remove_with_no_item_in_cache_Should_do_nothing();
    }

    [Fact]
    [Trait("Category", "Remove")]
    public void Remove_with_item_in_cache_Should_remove()
    {
        _fixture.Remove_with_item_in_cache_Should_remove();
    }

    [Fact]
    [Trait("Category", "RemoveAsync")]
    public async Task RemoveAsync_with_no_item_in_cache_Should_do_nothing()
    {
        await _fixture.RemoveAsync_with_no_item_in_cache_Should_do_nothing();
    }

    [Fact]
    [Trait("Category", "RemoveAsync")]
    public async Task RemoveAsync_with_item_in_cache_Should_remove_item()
    {
        await _fixture.RemoveAsync_with_item_in_cache_Should_remove_item();
    }
}
