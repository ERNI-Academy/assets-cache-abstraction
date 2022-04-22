namespace ErniAcademy.Cache.Contracts;

public interface ICacheManager
{
    /// <summary>
    /// Get an Item from cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="key">the unique identifier key</param>
    /// <returns>TItem instance. default(TItem) if not found</returns>
    TItem Get<TItem>(string key);

    /// <summary>
    /// Get an Item from cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="key">the unique identifier key</param>
    /// <param name="factory">if the item is not in cache this factory will be invoked to get the item</param>
    /// <returns>TItem instance. default(TItem) if not found</returns>
    TItem GetOrAdd<TItem>(string key, Func<TItem> factory, ICacheOptions options = null);

    /// <summary>
    /// Get an Item from cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="key">the unique identifier key</param>
    /// <returns>Task<TItem>. default(TItem) if not found</returns>
    Task<TItem> GetAsync<TItem>(string key);

    /// <summary>
    /// Get an Item from cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="key">the unique identifier key</param>
    /// <param name="factory">if the item is not in cache this factory will be invoked to get the item</param>
    /// <returns>Task<TItem>. default(TItem) if not found</returns>
    Task<TItem> GetOrAddAsync<TItem>(string key, Func<Task<TItem>> factory, ICacheOptions options = null);

    /// <summary>
    /// Set an Item into the cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="key">the unique identifier key</param>
    /// <param name="value">the instance of TItem to be setted into the cache</param>
    /// <param name="options">the cache options for this item</param>
    void Set<TItem>(string key, TItem value, ICacheOptions options = null);

    /// <summary>
    /// Set an Item into the cache
    /// </summary>
    /// <typeparam name="TItem">generic type of the item</typeparam>
    /// <param name="key">the unique identifier key</param>
    /// <param name="value">the instance of TItem to be setted into the cache</param>
    /// <param name="options">the cache options for this item</param>
    Task SetAsync<TItem>(string key, TItem value, ICacheOptions options = null);

    /// <summary>
    /// Check if there is an item with that key into the cache
    /// </summary>
    /// <param name="key">the unique identifier key</param>
    /// <returns>true if exists, false otherwise</returns>
    bool Exists(string key);

    /// <summary>
    /// Check if there is an item with that key into the cache
    /// </summary>
    /// <param name="key">the unique identifier key</param>
    /// <returns>true if exists, false otherwise</returns>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// Remove an item from cache
    /// </summary>
    /// <param name="key">the unique identifier key</param>
    void Remove(string key);

    /// <summary>
    /// Remove an item from cache
    /// </summary>
    /// <param name="key">the unique identifier key</param>
    Task RemoveAsync(string key);
}
