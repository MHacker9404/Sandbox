using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BaseWebApi.Infrastructure;
using BaseWebApi.Infrastructure.Enrichers;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BaseWebApi
{
    public static class Program
    {
        public static async Task Main( string[ ] args )
        {
            try
            {
                var currentEnv = Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" );
                var build = CreateHostBuilder( args ).Build( );
                Log.Information( $"Starting web host - {currentEnv}" );
                await build.RunAsync( );
            }
            catch ( Exception ex )
            {
                // Log.Logger will likely be internal type "Serilog.Core.Pipeline.SilentLogger".
                if ( Log.Logger == null || Log.Logger.GetType( ).Name == "SilentLogger" )
                    // Loading configuration or Serilog failed.
                    // This will create a logger that can be captured by AWS logger.
                    // To enable AWS logger, in AWS Portal:
                    // 1. Go to WebApp
                    // 2. App Service logs
                    // 3. Enable "Application Logging (Filesystem)", "Application Logging (Filesystem)" and "Detailed error messages"
                    // 4. Set Retention Period (Days) to 10 or similar value
                    // 5. Save settings
                    // 6. Under Overview, restart web app
                    // 7. Go to Log Stream and observe the logs
                    Log.Logger = new LoggerConfiguration( )
                                 .MinimumLevel.Debug( )
                                 .WriteTo.Console( )
                                 .CreateLogger( );

                Log.Fatal( ex, "Host terminated unexpectedly" );
            }
            finally
            {
                Log.CloseAndFlush( );
            }
        }

        public static IHostBuilder CreateHostBuilder( string[ ] args ) =>
            Host.CreateDefaultBuilder( args )
                .ConfigureWebHostDefaults( webBuilder =>
                {
                    webBuilder.UseStartup<Startup>( )
                              .CaptureStartupErrors( true )
                              .ConfigureAppConfiguration( config =>
                              {
                                                            // Used for local settings like connection strings.
                                                            config.AddJsonFile( "appsettings.Local.json", true );
                              } )
                              .UseLamar( )
                              .UseSerilog( ( hostingContext, loggerConfiguration ) =>
                              {
                                  loggerConfiguration
                                                                .ReadFrom.Configuration( hostingContext.Configuration )
                                                                .Enrich.FromLogContext( )
                                                                .Enrich.WithMachineName( )
                                                                .Enrich.WithThreadId( )
                                                                .Enrich.WithCorrelationId( )
                                                                .Enrich.WithCaller( )
                                                                .Enrich.WithProperty(
                                                                    "ApplicationName"
                                                                    , typeof( Program ).Assembly.GetName( ).Name )
                                                                .Enrich.WithProperty( "Environment", hostingContext.HostingEnvironment );

#if DEBUG
                                                            // Used to filter out potentially bad data due debugging.
                                                            // Very useful when doing Seq dashboards and want to remove logs under debugging session.
                                                            loggerConfiguration.Enrich.WithProperty( "DebuggerAttached"
                                                                                                    , Debugger.IsAttached );
#endif
                                                        } );
                } );
    }
}
