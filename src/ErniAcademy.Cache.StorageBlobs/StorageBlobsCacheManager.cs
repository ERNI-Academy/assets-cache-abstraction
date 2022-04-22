using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.StorageBlobs.Extensions;
using ErniAcademy.Cache.StorageBlobs.ClientProvider;
using ErniAcademy.Cache.StorageBlobs.Configuration;
using ErniAcademy.Serializers.Contracts;
using Microsoft.Extensions.Options;

namespace ErniAcademy.Cache.StorageBlobs;

public class StorageBlobsCacheManager : ICacheManager
{
    private readonly Lazy<BlobContainerClient> _blobContainerClientLazy;
    private readonly ISerializer _serializer;
    private readonly ICacheOptions _defaultOptions;

    public StorageBlobsCacheManager(
        IBlobContainerClientProvider blobContainerClientProvider,
        ISerializer serializer,
        IOptionsMonitor<StorageBlobsCacheOptions> options)
    {
        _blobContainerClientLazy = new Lazy<BlobContainerClient>(() => blobContainerClientProvider.GetClient());
        _serializer = serializer;
        _defaultOptions = new CacheOptions();
        _defaultOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(options.CurrentValue.TimeToLiveInSeconds);
    }

    public TItem Get<TItem>(string key) => GetAsync<TItem>(key).GetAwaiter().GetResult();

    public async Task<TItem> GetAsync<TItem>(string key, CancellationToken cancellationToken = default)
    {
        var blobClient = GetBlobClient(key);

        Response<BlobDownloadStreamingResult> blobResponse;

        try
        {
            blobResponse = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        }
        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            return default(TItem);
        }

        if(blobResponse.Value.HasExpired())
        {
            await RemoveAsync(key, cancellationToken);
            return default(TItem);
        }

        var result = await _serializer.DeserializeFromStreamAsync<TItem>(blobResponse.Value.Content, cancellationToken);
        blobResponse.Value.Dispose();
        return result;
    }

    public void Set<TItem>(string key, TItem value, ICacheOptions options = null) => SetAsync<TItem>(key, value, options).GetAwaiter().GetResult();

    public async Task SetAsync<TItem>(string key, TItem value, ICacheOptions options = null, CancellationToken cancellationToken = default)
    {
        GuardValue(value);

        await using var stream = new MemoryStream();
        await _serializer.SerializeToStreamAsync(value, stream, cancellationToken);
        stream.Seek(0, SeekOrigin.Begin);
        
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = _serializer.ContentType };

        var blobClient = GetBlobClient(key);
        await blobClient.UploadAsync(stream, blobHttpHeaders, (options ?? _defaultOptions).ToMetadata(), cancellationToken: cancellationToken);
    }

    public bool Exists(string key) => ExistsAsync(key).GetAwaiter().GetResult();

    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        var blobClient = GetBlobClient(key);
        try
        {
            var response = await blobClient.ExistsAsync(cancellationToken);
            return response.Value;
        }
        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            return false;
        }
    }

    public void Remove(string key) => RemoveAsync(key).GetAwaiter().GetResult();

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var blobClient = GetBlobClient(key);
        return blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    internal BlobClient GetBlobClient(string key)
    {
        GuardKey(key);
        return _blobContainerClientLazy.Value.GetBlobClient(key);
    }

    internal static void GuardKey(string key)
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

    internal static void GuardValue<TItem>(TItem value)
    {
        if (EqualityComparer<TItem>.Default.Equals(value, default))
        {
            throw new ArgumentException($"cache a default value is not allowed", nameof(value));
        }
    }
}
