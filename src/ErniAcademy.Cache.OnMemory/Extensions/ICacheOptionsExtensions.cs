using ErniAcademy.Cache.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace ErniAcademy.Cache.OnMemory.Extensions;

public static class ICacheOptionsExtensions
{
    public static MemoryCacheEntryOptions ToMemoryCacheEntryOptions(this ICacheOptions options)
    {
        if(options == null) return null;

        return new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = options.AbsoluteExpiration,
            SlidingExpiration = options.SlidingExpiration,
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
        };
    }
}
