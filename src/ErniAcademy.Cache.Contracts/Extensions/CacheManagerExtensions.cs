using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace ErniAcademy.Cache.Contracts;

public static class CacheManagerExtensions
{
    private static readonly ConcurrentDictionary<string, Lazy<SemaphoreSlim>> _semaphores = new ConcurrentDictionary<string, Lazy<SemaphoreSlim>>();
    private static readonly ObjectIDGenerator _objectIdGenerator = new ObjectIDGenerator();

    /// <summary>
    /// Get from cache if is present in cache. Otherwise will invoke factory to get the value and will return value after setted into cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="cacheManager">ICacheManager instance to be extended</param>
    /// <param name="key">the unique identifier key</param>
    /// <param name="factory">factory to be invoked to get TItem value if is not in cache</param>
    /// <param name="options">the cache options for this item</param>
    /// <returns>TItem. default(TItem) if not found</returns>
    public static TItem GetOrAdd<TItem>(
        this ICacheManager cacheManager,
        string key,
        Func<TItem> factory,
        ICacheOptions options = null)
    {
        TItem item;
        var semaphoreKey = _objectIdGenerator.GetId(cacheManager, out _) + key;
        try
        {
            WaitSemaphore(semaphoreKey);
            item = cacheManager.Get<TItem>(key);
            if (EqualityComparer<TItem>.Default.Equals(item, default))
            {
                item = factory.Invoke();
                cacheManager.Set(key, item, options);
            }
        }
        finally
        {
            ReleaseSemaphore(semaphoreKey);
        }

        return item;
    }

    /// <summary>
    /// Get from cache if is present in cache. Otherwise will invoke factory to get the value and will return value after setted into cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="cacheManager">ICacheManager instance to be extended</param>
    /// <param name="key">the unique identifier key</param>
    /// <param name="factory">factory to be invoked to get TItem value if is not in cache</param>
    /// <param name="options">the cache options for this item</param>
    /// <returns>Task<TItem>. default(TItem) if not found</returns>
    public static async Task<TItem> GetOrAddAsync<TItem>(
        this ICacheManager cacheManager,
        string key,
        Func<Task<TItem>> factory,
        ICacheOptions options = null)
    {
        TItem item;
        var semaphoreKey = _objectIdGenerator.GetId(cacheManager, out _) + key;
        try
        {
            await GetOrAddSemaphore(semaphoreKey).WaitAsync();
            Task<TItem> task = cacheManager.GetAsync<TItem>(key);
            item = task.IsCompletedSuccessfully ? task.Result : await task;
            if (EqualityComparer<TItem>.Default.Equals(item, default))
            {
                item = await factory.Invoke();
                Task taskSet = cacheManager.SetAsync(key, item, options);
                if (!taskSet.IsCompletedSuccessfully)
                {
                    await taskSet;
                }
            }
        }
        finally
        {
            ReleaseSemaphore(semaphoreKey);
        }

        return item;
    }

    private static SemaphoreSlim GetOrAddSemaphore(string key)
    {
        var lazySemaphore = _semaphores.GetOrAdd(key, k => new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(1, 1)));
        return lazySemaphore.Value;
    }

    private static void WaitSemaphore(string key) => GetOrAddSemaphore(key).Wait();

    private static void ReleaseSemaphore(string key) => GetOrAddSemaphore(key).Release();
}
