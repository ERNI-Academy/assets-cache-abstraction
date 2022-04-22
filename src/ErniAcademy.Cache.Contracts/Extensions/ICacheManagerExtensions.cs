using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace ErniAcademy.Cache.Contracts;

public static class ICacheManagerExtensions
{
    private static readonly ConcurrentDictionary<string, Lazy<SemaphoreSlim>> _semaphores = new ConcurrentDictionary<string, Lazy<SemaphoreSlim>>();
    private static readonly ObjectIDGenerator _objectIdGenerator = new ObjectIDGenerator();

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
