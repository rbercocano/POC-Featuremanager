using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        private static IConfiguration _configuration;
        private static IConfigurationRefresher _refresher;
        public static async Task Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddAzureAppConfiguration(options =>
                {
                    options.Connect("Endpoint=https://poc-core.azconfig.io;Id=Lz6A-l6-s0:mMC85TF+9U8kZ4pgmtPE;Secret=D00IZT2F/4xds1SBXrRBkcwigE/QuXdsbslCCFLccA0=")
                            .ConfigureRefresh(c =>
                              {
                                  c.Register("cqrs", true).SetCacheExpiration(TimeSpan.FromSeconds(2));
                              })
                           .UseFeatureFlags();
                    _refresher = options.GetRefresher();
                }).Build();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration).AddFeatureManagement().AddFeatureFilter<ContextualTargetingFilter>();
            TargetingContext targetingContext = new TargetingContext
            {
                Groups = new List<string>() { "Mercy" }
            };
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IFeatureManager featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

                if (await featureManager.IsEnabledAsync("Beta"))
                {
                    Console.WriteLine("Welcome to the beta!");
                }
                if (await featureManager.IsEnabledAsync("Beta", targetingContext))
                {
                    Console.WriteLine("Welcome to the CQRS!");
                }
            }

            Console.WriteLine("Hello World!");
            Console.WriteLine("Press any key to continue ...");
            Console.Read();
        }
    }
}
