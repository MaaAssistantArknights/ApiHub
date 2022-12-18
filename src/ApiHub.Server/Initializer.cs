// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Enums;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ApiHub.Server;

public static class Initializer
{
    public static IConfigurationRoot GetConfiguration()
    {
        var builder = new ConfigurationBuilder();
        
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APOLLO_DISABLED")))
        {
            builder.AddApollo(new ApolloOptions
                {
                    AppId = Environment.GetEnvironmentVariable("APOLLO_APP_ID") ?? "api-hub",
                    MetaServer = Environment.GetEnvironmentVariable("APOLLO_META_SERVER") ?? "http://localhost:8080",
                    Cluster = Environment.GetEnvironmentVariable("APOLLO_CLUSTER") ?? "default",
                    Namespaces = Environment.GetEnvironmentVariable("APOLLO_NAMESPACES")?.Split(',') ?? new[] { "application" },
                    Secret = Environment.GetEnvironmentVariable("APOLLO_SECRET"),
                    Env = Enum.TryParse<Env>(Environment.GetEnvironmentVariable("APOLLO_ENV"), out var env) ? env : Env.Dev
                });
        }
        else
        {
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        }
        
        return builder.Build();
    }

    public static ILogger GetLogger(IConfigurationRoot configuration)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}
