using Microsoft.Extensions.Options;
using ErniAcademy.Cache.StorageBlobs.Configuration;
using Azure.Storage.Blobs;

namespace ErniAcademy.Cache.StorageBlobs.ClientProvider;

/// <summary>
/// Provider to build a BlobContainerClient based on connection string settings
/// </summary>
public class ConnectionStringProvider : IBlobContainerClientProvider
{
    private readonly IOptionsMonitor<ConnectionStringOptions> _options;
    private readonly BlobClientOptions _blobOptions;

    /// <summary>
    /// Initializes a new instance of the ConnectionStringProvider class.
    /// </summary>
    /// <param name="options">IOptionsMonitor of ConnectionStringOptions settings</param>
    /// <param name="blobOptions">Optional client options that define the transport pipeline policies for authentication, retries, etc., that are applied to every request.</param>
    public ConnectionStringProvider(IOptionsMonitor<ConnectionStringOptions> options, BlobClientOptions blobOptions = null)
    {
        _options = options;
        _blobOptions = blobOptions;
    }

    public BlobContainerClient GetClient() => new BlobContainerClient(_options.CurrentValue.ConnectionString, _options.CurrentValue.ContainerName, _blobOptions);
}
