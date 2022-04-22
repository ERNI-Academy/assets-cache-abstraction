using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.StorageBlobs.Configuration;

namespace ErniAcademy.Cache.StorageBlobs.Extensions;

public static class ICacheOptionsExtensions
{
    public static Dictionary<string, string> ToMetadata(this ICacheOptions options)
    {
        if (options == null) return new Dictionary<string, string>();

        var dateTimeOffset = DateTimeOffset.UtcNow;
        var expiration = options.GetAbsoluteExpiration(dateTimeOffset) ?? dateTimeOffset;

        return new Dictionary<string, string>
                {
                    { Constants.ExpiredAt, expiration.UtcDateTime.ToString("o") }
                };
    }

    public static DateTimeOffset? GetAbsoluteExpiration(this ICacheOptions options, DateTimeOffset creationTime)
    {
        if (options.AbsoluteExpiration.HasValue && options.AbsoluteExpiration <= creationTime)
        {
            throw new ArgumentOutOfRangeException(
                nameof(CacheOptions.AbsoluteExpiration),
                options.AbsoluteExpiration.Value,
                "The absolute expiration value must be in the future.");
        }

        var absoluteExpiration = options.AbsoluteExpirationRelativeToNow.HasValue
            ? creationTime + options.AbsoluteExpirationRelativeToNow
            : options.AbsoluteExpiration;

        return absoluteExpiration;
    }
}
