using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.Contracts.Extensions;
using ErniAcademy.Cache.StorageBlobs.Configuration;

namespace ErniAcademy.Cache.StorageBlobs.Extensions;

public static class CacheOptionsExtensions
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
}