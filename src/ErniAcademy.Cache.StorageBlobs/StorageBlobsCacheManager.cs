using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ErniAcademy.Cache.Contracts;
using ErniAcademy.Cache.StorageBlobs.Extensions;
using ErniAcademy.Events.StorageBlobs.ClientProvider;
using ErniAcademy.Serializers.Contracts;

namespace ErniAcademy.Cache.StorageBlobs;

public class StorageBlobsCacheManager : ICacheManager
{
    private readonly Lazy<BlobContainerClient> _blobContainerClientLazy;
    private readonly ISerializer _serializer;

    public StorageBlobsCacheManager(
        IBlobContainerClientProvider blobContainerClientProvider,
        ISerializer serializer)
    {
        _blobContainerClientLazy = new Lazy<BlobContainerClient>(() => blobContainerClientProvider.GetClient());
        _serializer = serializer;
    }

    public TItem Get<TItem>(string key) => GetAsync<TItem>(key).GetAwaiter().GetResult();

    public async Task<TItem> GetAsync<TItem>(string key)
    {
        var blobClient = GetBlobClient(key);

        Response<BlobDownloadStreamingResult> blobResponse;

        try
        {
            blobResponse = await blobClient.DownloadStreamingAsync();
        }
        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            return default(TItem);
        }

        if(blobResponse.Value.HasExpired())
        {
            await RemoveAsync(key);
            return default(TItem);
        }

        var result = await _serializer.DeserializeFromStreamAsync<TItem>(blobResponse.Value.Content);
        blobResponse.Value.Dispose();
        return result;
    }

    public void Set<TItem>(string key, TItem value, ICacheOptions options = null) => SetAsync<TItem>(key, value, options).GetAwaiter().GetResult();

    public async Task SetAsync<TItem>(string key, TItem value, ICacheOptions options = null)
    {
        await using var stream = new MemoryStream();
        await _serializer.SerializeToStreamAsync(value, stream);
        stream.Seek(0, SeekOrigin.Begin);
        
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = _serializer.ContentType };

        var blobClient = GetBlobClient(key);
        await blobClient.UploadAsync(stream, blobHttpHeaders, options.ToMetadata());
    }

    public bool Exists(string key) => ExistsAsync(key).GetAwaiter().GetResult();

    public async Task<bool> ExistsAsync(string key)
    {
        var blobClient = GetBlobClient(key);
        try
        {
            var response = await blobClient.ExistsAsync();
            return response.Value;
        }
        catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
        {
            return false;
        }
    }

    public void Remove(string key) => RemoveAsync(key).GetAwaiter().GetResult();

    public Task RemoveAsync(string key)
    {
        var blobClient = GetBlobClient(key);
        return blobClient.DeleteIfExistsAsync();
    }

    internal BlobClient GetBlobClient(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException($"invalid {nameof(key)} is mandatory", nameof(key));
        }

        return _blobContainerClientLazy.Value.GetBlobClient(key);
    }
}
