namespace ErniAcademy.Cache.StorageBlobs.Configuration;

public class StorageBlobsCacheOptions
{
    /// <summary>
    /// The time to live in seconds for cached blobs. Default to 3600 secs = 1 hour
    /// </summary>
    public int TimeToLiveInSeconds { get; set; } = 3600;
}