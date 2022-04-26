using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.OnMemory.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ErniAcademy.Cache.OnMemory;

public class OnMemoryCacheManager : ICacheManager
{
    private readonly MemoryCache _memoryCache;
    private readonly ILogger _logger;

    public OnMemoryCacheManager(ILoggerFactory loggerFactory)
        : this(new MemoryCacheOptions(), loggerFactory)
    {
    }

    public OnMemoryCacheManager(MemoryCacheOptions options, ILoggerFactory loggerFactory)
    {
        _memoryCache = new MemoryCache(options);
        _logger = loggerFactory.CreateLogger(nameof(OnMemoryCacheManager));
    }

    public TItem Get<TItem>(string key)
    {
        CacheGuard.GuardKey(key);

        var value = _memoryCache.Get<TItem>(key);

        var cacheHit = value == null;
        _logger.Log(LogLevel.Information, "Cache get '{key}' hit: {cacheHit}", key, cacheHit);

        return value;
    }

    public Task<TItem> GetAsync<TItem>(string key, CancellationToken cancellationToken = default) => Task.FromResult(Get<TItem>(key));

    public void Set<TItem>(string key, TItem value, ICacheOptions options = null)
    {
        CacheGuard.GuardKey(key);
        CacheGuard.GuardValue(value);

        _memoryCache.Set<TItem>(key, value, options.ToMemoryCacheEntryOptions());

        _logger.Log(LogLevel.Information, "Cache set '{key}' options: {options}", key, options?.ToString());
    }

    public Task SetAsync<TItem>(string key, TItem value, ICacheOptions options = null, CancellationToken cancellationToken = default)
    {
        Set<TItem>(key, value, options);
        return Task.CompletedTask;
    }

    public bool Exists(string key)
    {
        CacheGuard.GuardKey(key);
        return _memoryCache.Get(key) != null;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default) => Task.FromResult(Exists(key));

    public void Remove(string key)
    {
        CacheGuard.GuardKey(key);
        _memoryCache.Remove(key);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }
}
