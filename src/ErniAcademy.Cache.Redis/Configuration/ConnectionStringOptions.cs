using System.ComponentModel.DataAnnotations;

namespace ErniAcademy.Cache.Redis.Configuration;

public class ConnectionStringOptions
{
    /// <summary>
    /// The connection string to use for connecting to the Redis database.
    /// </summary>
    [Required] 
    public string ConnectionString { get; set; }
}

