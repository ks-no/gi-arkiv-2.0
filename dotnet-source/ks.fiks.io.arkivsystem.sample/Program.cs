﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace ks.fiks.io.arkivsystem.sample
{
    class Program
    {
        static async Task Main(string[] args)
        { 
            await new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(CreateAppSettings(hostContext.Configuration));
                    services.AddHostedService<ArkivService>();
                }).ConfigureHostConfiguration((configHost) =>
                {
                    configHost.AddEnvironmentVariables("DOTNET_");
                })
                .ConfigureAppConfiguration((hostBuilder, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddEnvironmentVariables("fiksArkivsystemMock_");
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostBuilder.HostingEnvironment.EnvironmentName}.json", optional: true);
                })
                .RunConsoleAsync();
        }

        private static AppSettings CreateAppSettings(IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            
            // Available from maskinporten-config config-map in k8s cluster
            var maskinportenClientId = Environment.GetEnvironmentVariable("MASKINPORTEN_CLIENT_ID");
            
            if (!string.IsNullOrEmpty(maskinportenClientId))
            {
                appSettings.FiksIOConfig.MaskinPortenIssuer = maskinportenClientId;
            }
            
            return appSettings;
        }

       
    }
}
