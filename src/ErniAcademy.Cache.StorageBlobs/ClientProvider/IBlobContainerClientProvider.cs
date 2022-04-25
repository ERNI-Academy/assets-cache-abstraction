
using Azure.Storage.Blobs;

namespace ErniAcademy.Cache.StorageBlobs.ClientProvider;

/// <summary>
/// Contract to provide an instance of BlobContainerClient
/// </summary>
public interface IBlobContainerClientProvider
{
    /// <summary>
    /// Get BlobContainerClient
    /// </summary>
    /// <returns>BlobContainerClient instance</returns>
    BlobContainerClient GetClient();
}
