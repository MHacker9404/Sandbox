using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Lamar.Microsoft.DependencyInjection;
using Marketplace.WebApi.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;

namespace Marketplace.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Build Config
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", false, false)
                                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true, false)
                                .AddEnvironmentVariables()
                                .Build();

            //Configure logger
            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(configuration)
                         .Enrich.WithMachineName()
                         .Enrich.FromLogContext()
                         .Enrich.WithThreadId()
                         .Enrich.WithCorrelationId()
                         .Enrich.WithCaller()
                         .CreateLogger();

            SelfLog.Enable(msg => Debug.WriteLine(msg));
            SelfLog.Enable(Console.Error);

            try
            {
                Log.Information($"Starting web host - {currentEnv}");
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Web Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSerilog((hc, lc) =>
                //            {
                //                lc.ReadFrom.Configuration(hc.Configuration).Enrich.WithMachineName().Enrich.FromLogContext().Enrich.WithThreadId().Enrich
                //                  .WithCorrelationId();
                //            })
                .UseSerilog(Log.Logger)
                .UseLamar()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}