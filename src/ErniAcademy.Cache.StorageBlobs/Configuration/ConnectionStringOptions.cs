using System.ComponentModel.DataAnnotations;

namespace ErniAcademy.Cache.StorageBlobs.Configuration;

public class ConnectionStringOptions
{
    /// <summary>
    /// The connection string to use for connecting to the Storage.
    /// </summary>
    [Required]
    public string ConnectionString { get; set; }

    /// <summary>
    /// The container name to the Storage Blob.
    /// </summary>
    [Required]
    public string ContainerName { get; set; }
}
