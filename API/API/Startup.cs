using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Configuration;
using OrleansAW.Grains;
using OrleansAW.Grains.Interfaces;
using OrleansAW.Utils;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IClusterClient>(CreateClusterClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetService<ILogger<Startup>>();

            // TODO replace with your connection string
            // const string connectionString = "YOUR_CONNECTION_STRING_HERE";
            var client = new ClientBuilder()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IStartupGrain).Assembly))
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(StartupGrain).Assembly))
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Constants.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
               // .UseAzureStorageClustering(options => options.ConnectionString = connectionString)
               .UseLocalhostClustering()
               .Build();

            client.Connect(RetryFilter).GetAwaiter().GetResult();
            return client;

            async Task<bool> RetryFilter(Exception exception)
            {
                log?.LogWarning("Exception while attempting to connect to Orleans cluster: {Exception}", exception);
                await Task.Delay(TimeSpan.FromSeconds(2));
                return true;
            }
        }
    }
}
