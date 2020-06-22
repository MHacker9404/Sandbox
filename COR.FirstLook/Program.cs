using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace COR.FirstLook
{
    class Program
    {
        static void Main( string[ ] args )
        {
            var currentEnv = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
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
                         .CreateLogger();

            var user = new User("Phil Boyd"
                                , "123456789"
                                , new RegionInfo("US")
                                , new DateTimeOffset(new DateTime(1967, 11, 22), TimeSpan.FromHours(2)));

            var processor = new UserProcessor();

            var result = processor.Register(user);

            Log.Debug($"result", JsonConvert.SerializeObject(result));
        }
    }
}
