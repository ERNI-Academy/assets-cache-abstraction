using System.Text;

namespace ErniAcademy.Cache.Contracts;

public class CacheOptions : ICacheOptions
{
    private TimeSpan? _absoluteExpirationRelativeToNow;
    private TimeSpan? _slidingExpiration;

    public DateTimeOffset? AbsoluteExpiration { get; set; }

    public TimeSpan? AbsoluteExpirationRelativeToNow
    {
        get
        {
            return _absoluteExpirationRelativeToNow;
        }
        set
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(AbsoluteExpirationRelativeToNow), value, "The relative expiration value must be positive.");
            }

            _absoluteExpirationRelativeToNow = value;
        }
    }

    public TimeSpan? SlidingExpiration
    {
        get
        {
            return _slidingExpiration;
        }
        set
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(SlidingExpiration), value, "The sliding expiration value must be positive.");
            }
            _slidingExpiration = value;
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        if(AbsoluteExpiration.HasValue)
        {
            sb.Append($"{nameof(AbsoluteExpiration)}:{AbsoluteExpiration.Value.ToString("o")}.");
        }
        if (AbsoluteExpirationRelativeToNow.HasValue)
        {
            sb.Append($"{nameof(AbsoluteExpirationRelativeToNow)}:{AbsoluteExpirationRelativeToNow.Value.ToString()}.");
        }
        if (SlidingExpiration.HasValue)
        {
            sb.Append($"{nameof(SlidingExpiration)}:{SlidingExpiration.Value.ToString()}.");
        }

        return sb.ToString();
    }
}
