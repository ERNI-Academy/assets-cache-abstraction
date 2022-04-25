namespace ErniAcademy.Cache.Contracts;

public static class CacheGuard
{
    public static void GuardKey(string key)
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

    public static void GuardValue<TItem>(TItem value)
    {
        if (EqualityComparer<TItem>.Default.Equals(value, default))
        {
            throw new ArgumentException($"cache a default value is not allowed", nameof(value));
        }
    }
}
