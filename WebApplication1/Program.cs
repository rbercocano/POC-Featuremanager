using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        config.AddAzureAppConfiguration(options =>
                            options.Connect("Endpoint=https://poc-core.azconfig.io;Id=Lz6A-l6-s0:mMC85TF+9U8kZ4pgmtPE;Secret=D00IZT2F/4xds1SBXrRBkcwigE/QuXdsbslCCFLccA0=").UseFeatureFlags());
                    }).UseStartup<Startup>();
                });
    }
}
