using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.OnMemory.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace ErniAcademy.Cache.OnMemory;

public class OnMemoryCacheManager : ICacheManager
{
    private readonly MemoryCache _memoryCache;

    public OnMemoryCacheManager()
        : this(new MemoryCacheOptions())
    {
    }

    public OnMemoryCacheManager(MemoryCacheOptions options)
    {
        _memoryCache = new MemoryCache(options);
    }

    public TItem Get<TItem>(string key) => _memoryCache.Get<TItem>(key);

    public Task<TItem> GetAsync<TItem>(string key) => Task.FromResult(Get<TItem>(key));

    public void Set<TItem>(string key, TItem value, ICacheOptions options = null) => _memoryCache.Set<TItem>(key, value, options.ToMemoryCacheEntryOptions());

    public Task SetAsync<TItem>(string key, TItem value, ICacheOptions options = null)
    {
        Set<TItem>(key, value, options);
        return Task.CompletedTask;
    }

    public bool Exists(string key) => _memoryCache.Get(key) != null;

    public Task<bool> ExistsAsync(string key) => Task.FromResult(Exists(key));

    public void Remove(string key) => _memoryCache.Remove(key);

    public Task RemoveAsync(string key)
    {
        Remove(key);
        return Task.CompletedTask;
    }
}
