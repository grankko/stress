using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace Stress.Server
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void Main(string[] args)
        {
            var build = CreateHostBuilder(args).Build();
            ServiceProvider = build.Services;
            build.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
