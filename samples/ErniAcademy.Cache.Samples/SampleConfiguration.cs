using ErniAcademy.Serializers.Contracts;
using ErniAcademy.Serializers.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ErniAcademy.Cache.OnMemory.Extensions;
using ErniAcademy.Cache.Redis.Extensions;
using ErniAcademy.Cache.StorageBlobs.Extensions;

namespace ErniAcademy.Cache.Samples;

public static class SampleConfiguration
{
    /// <summary>
    /// this is a sample of how you can configure OnMemory cache, so later you can use dependency injection
    /// </summary>
    /// <param name="services">your service collection</param>
    public static void ConfigureOnMemory(IServiceCollection services)
    {
        services.AddCacheOnMemory();
    }

    /// <summary>
    /// this is a sample of how you can configure Redis cache, so later you can use dependency injection
    /// </summary>
    /// <param name="services">your service collection</param>
    /// <param name="configuration">your configuration</param>
    public static void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
    {
        ISerializer serializer = new JsonSerializer(); //please note that ISerializer serializer is not in the IoC thats why it may be the case that you want diferents serializers impl within your app

        ///please note that it is mandatory for you to have a configuration like this to match your sectionKey "Cache:Redis"
        ///Cache:{
        ///"Redis": {
        ///    "ConnectionString": "[put here your Redis connection string]"
        ///}
        services.AddCacheRedis(configuration, serializer, sectionKey: "Cache:Redis");
    }

    /// <summary>
    /// this is a sample of how you can configure StorageBlobs cache, so later you can use dependency injection
    /// </summary>
    /// <param name="services">your service collection</param>
    /// <param name="configuration">your configuration</param>
    public static void ConfigureStorageBlobs(IServiceCollection services, IConfiguration configuration)
    {
        ISerializer serializer = new JsonSerializer(); //please note that ISerializer serializer is not in the IoC thats why it may be the case that you want diferents serializers impl within your app

        ///please note that it is mandatory for you to have a configuration like this to match your sectionKey "Cache:StorageBlobs"
        ///Cache:{
        ///"StorageBlobs": {
        ///    "ConnectionString": "[put here your StorageBlobs connection string]"
        ///}
        services.AddCacheStorageBlobs(configuration, serializer, sectionKey: "Cache:StorageBlobs");
    }
}
