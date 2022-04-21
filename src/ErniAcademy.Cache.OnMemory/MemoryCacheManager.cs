using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.OnMemory.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ErniAcademy.Cache.OnMemory;

public class MemoryCacheManager : ICacheManager
{
    private readonly MemoryCache _memoryCache;

    public MemoryCacheManager(IOptions<MemoryCacheOptions> options)
    {
        _memoryCache = new MemoryCache(options);
    }

    public TItem Get<TItem>(string key) => _memoryCache.Get<TItem>(key);

    public TItem GetOrAdd<TItem>(string key, Func<TItem> factory) => _memoryCache.GetOrCreate<TItem>(key, (cacheEntry) => { return factory(); });

    public Task<TItem> GetAsync<TItem>(string key) => Task.FromResult(Get<TItem>(key));

    public Task<TItem> GetOrAddAsync<TItem>(string key, Func<Task<TItem>> factory) => _memoryCache.GetOrCreateAsync<TItem>(key, (cacheEntry) => { return factory(); });

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
