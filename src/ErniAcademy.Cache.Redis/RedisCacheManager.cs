using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.Redis.Configuration;
using ErniAcademy.Serializers.Contracts;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ErniAcademy.Cache.Redis;

public class RedisCacheManager : ICacheManager
{
    private readonly Lazy<IDatabase> _databaseLazy;
    private readonly ISerializer _serializer;
    private readonly ICacheOptions _defaultOptions;

    public RedisCacheManager(
        IConnectionMultiplexerProvider provider,
        ISerializer serializer,
        IOptionsMonitor<RedisCacheOptions> options)
    {
        _serializer = serializer;
        _databaseLazy = new Lazy<IDatabase>(() => provider.Connection.GetDatabase());
        _defaultOptions = new CacheOptions();
        _defaultOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(options.CurrentValue.TimeToLiveInSeconds);
    }

    public TItem Get<TItem>(string key) => GetAsync<TItem>(key).GetAwaiter().GetResult();

    public async Task<TItem> GetAsync<TItem>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _databaseLazy.Value.StringGetAsync(key);
        return _serializer.DeserializeFromString<TItem>(value.ToString());
    }

    public void Set<TItem>(string key, TItem value, ICacheOptions options = null) => SetAsync<TItem>(key, value).GetAwaiter().GetResult();

    public async Task SetAsync<TItem>(string key, TItem value, ICacheOptions options = null, CancellationToken cancellationToken = default)
    {
        GuardKey(key);
        GuardValue(value);

        var valueStr = _serializer.SerializeToString(value);
        await _databaseLazy.Value.StringSetAsync(key, valueStr, (options ?? _defaultOptions).To());
    }

    public bool Exists(string key) => ExistsAsync(key).GetAwaiter().GetResult();

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        GuardKey(key);
        return _databaseLazy.Value.KeyExistsAsync(key);
    }

    public void Remove(string key) => RemoveAsync(key).GetAwaiter().GetResult();

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        GuardKey(key);
        return _databaseLazy.Value.KeyDeleteAsync(key);
    }

    internal static void GuardKey(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException($"invalid {nameof(key)}", nameof(key));
        }
    }

    internal static void GuardValue<TItem>(TItem value)
    {
        if (EqualityComparer<TItem>.Default.Equals(value, default))
        {
            throw new ArgumentException($"cache a default value is not allowed", nameof(value));
        }
    }
}
