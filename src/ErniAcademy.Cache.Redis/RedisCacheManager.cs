using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.Contracts.Extensions;
using ErniAcademy.Cache.Redis.Configuration;
using ErniAcademy.Serializers.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ErniAcademy.Cache.Redis;

public class RedisCacheManager : ICacheManager
{
    private readonly Lazy<IDatabase> _databaseLazy;
    private readonly ISerializer _serializer;
    private readonly ILogger _logger;
    private readonly ICacheOptions _defaultOptions;

    public RedisCacheManager(
        IConnectionMultiplexerProvider provider,
        ISerializer serializer,
        IOptionsMonitor<RedisCacheOptions> options,
        ILoggerFactory loggerFactory)
    {
        _serializer = serializer;
        _logger = loggerFactory.CreateLogger(nameof(RedisCacheManager));
        _databaseLazy = new Lazy<IDatabase>(() => {
            _logger.Log(LogLevel.Information, "Connecting to Redis database");
            return provider.Connection.GetDatabase();
        });
        _defaultOptions = new CacheOptions();
        _defaultOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(options.CurrentValue.TimeToLiveInSeconds);
    }

    public TItem Get<TItem>(string key) => GetAsync<TItem>(key).GetAwaiter().GetResult();

    public async Task<TItem> GetAsync<TItem>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _databaseLazy.Value.StringGetAsync(key);
        var cacheHit = value.HasValue && !value.IsNullOrEmpty;

        _logger.Log(LogLevel.Information, "Cache get '{key}' hit: {cacheHit}", key, cacheHit);

        if (!cacheHit)
        {
            return default(TItem);
        }

        return _serializer.DeserializeFromString<TItem>(value.ToString());
    }

    public void Set<TItem>(string key, TItem value, ICacheOptions options = null) => SetAsync<TItem>(key, value, options).GetAwaiter().GetResult();

    public async Task SetAsync<TItem>(string key, TItem value, ICacheOptions options = null, CancellationToken cancellationToken = default)
    {
        CacheGuard.GuardKey(key);
        CacheGuard.GuardValue(value);

        var currentOptions = (options ?? _defaultOptions);
        var valueStr = _serializer.SerializeToString(value);

        await _databaseLazy.Value.StringSetAsync(key, valueStr, expiry: currentOptions.GetExpiration(DateTimeOffset.UtcNow), flags: CommandFlags.FireAndForget);

        _logger.Log(LogLevel.Information, "Cache set '{key}' options: {currentOptions}", key, currentOptions.ToString());
    }

    public bool Exists(string key) => ExistsAsync(key).GetAwaiter().GetResult();

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        CacheGuard.GuardKey(key);
        return _databaseLazy.Value.KeyExistsAsync(key);
    }

    public void Remove(string key) => RemoveAsync(key).GetAwaiter().GetResult();

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        CacheGuard.GuardKey(key);
        return _databaseLazy.Value.KeyDeleteAsync(key);
    }
}
