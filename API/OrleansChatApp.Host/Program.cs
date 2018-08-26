using System;
using System.IO;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansAW.Grains;
using OrleansAW.Grains.Interfaces;
using OrleansAW.Utils;

namespace OrleansAW.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Constants.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .ConfigureAppConfiguration((context, confBuilder) => confBuilder.AddJsonFile("appSettings.json"))
                .UseLocalhostClustering()
                //.ConfigureEndpoints(IPAddress.Loopback, siloPort: 11111, gatewayPort: 30000)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IStartupGrain).Assembly).WithReferences())
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(StartupGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var silo = builder.Build();
            silo.StartAsync().Wait();

            Console.WriteLine("Press Enter to close.");
            Console.ReadLine();

            // shut the silo down after we are done.
            silo.StopAsync().Wait();
        }

    }
}
