using ErniAcademy.Cache.Samples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IServiceCollection services = new ServiceCollection();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

services.AddLogging(builder => builder.AddConsole());

services.AddSingleton<SampleServiceThatUsesCache>();

Console.WriteLine("type '1' for OnMemory");
Console.WriteLine("type '2' for Redis");
Console.WriteLine("type '3' for StorageBlobs");

var line = Console.ReadLine();

switch (line)
{
    case "1": { SampleConfiguration.ConfigureOnMemory(services); break; }
    case "2": { SampleConfiguration.ConfigureRedis(services, configuration); break; }
    case "3": { SampleConfiguration.ConfigureStorageBlobs(services, configuration); break; }
    default: { Console.WriteLine($"invalid type {line}"); break; }
}

var provider = services.BuildServiceProvider();

var service = provider.GetRequiredService<SampleServiceThatUsesCache>();

await service.RunAsync();