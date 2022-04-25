using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ErniAcademy.Cache.IntegrationTests.Utils;

internal static class ConfigurationHelper
{
    public static IConfiguration Get()
    {
        var tempConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("tests.settings.Development.json", optional: true)
                .Build();

        var isDevelopment = tempConfig.GetValue<string>("Environment") == "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile(isDevelopment ? "tests.settings.Development.json" : "tests.settings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        return configuration;
    }
}