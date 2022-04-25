using Azure.Storage.Blobs.Models;
using ErniAcademy.Cache.StorageBlobs.Configuration;

namespace ErniAcademy.Cache.StorageBlobs.Extensions;

internal static class BlobDownloadStreamingResultExtensions
{
    internal static bool HasExpired(this BlobDownloadStreamingResult result)
    {
        if (!result.Details.Metadata.ContainsKey(Constants.ExpiredAt))
        {
            return false;
        }

        var expireAt = DateTime.Parse(result.Details.Metadata[Constants.ExpiredAt]);
        return expireAt.ToUniversalTime() <= DateTime.UtcNow;
    }
}
