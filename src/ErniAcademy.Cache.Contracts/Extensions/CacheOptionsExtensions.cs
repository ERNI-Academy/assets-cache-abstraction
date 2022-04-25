namespace ErniAcademy.Cache.Contracts.Extensions;

public static class CacheOptionsExtensions
{
    public static DateTimeOffset? GetAbsoluteExpiration(this ICacheOptions options, DateTimeOffset creationTime)
    {
        if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                options.AbsoluteExpiration.Value,
                $"The {nameof(CacheOptions.AbsoluteExpiration)} value must be in the future.");
        }

        var absoluteExpiration = options.AbsoluteExpirationRelativeToNow.HasValue
            ? creationTime + options.AbsoluteExpirationRelativeToNow
            : options.AbsoluteExpiration;

        return absoluteExpiration;
    }

    public static TimeSpan? GetExpiration(this ICacheOptions options, DateTimeOffset creationTime)
    {
        if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                options.AbsoluteExpiration.Value,
                $"The {nameof(CacheOptions.AbsoluteExpiration)} value must be in the future.");
        }

        var result = options.AbsoluteExpirationRelativeToNow.HasValue
            ? options.AbsoluteExpirationRelativeToNow.Value
            : options.AbsoluteExpiration - creationTime;

        return result;
    }
}