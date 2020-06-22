using System;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace COR.PaymentProcessing
{
    class Program
    {
        static void Main( string[ ] args )
        {
            var currentEnv = Environment.GetEnvironmentVariable( "NETCORE_ENVIRONMENT" );
            var configuration = new ConfigurationBuilder( )
                                .AddJsonFile( "appsettings.json", false, false )
                                .AddJsonFile( $"appsettings.{currentEnv}.json", optional: true, false )
                                .AddEnvironmentVariables( )
                                .Build( );

            //Configure logger
            Log.Logger = new LoggerConfiguration( )
                         .ReadFrom.Configuration( configuration )
                         .Enrich.WithMachineName( )
                         .Enrich.FromLogContext( )
                         .Enrich.WithThreadId( )
                         .Enrich.WithCorrelationId( )
                         .CreateLogger( );
        }
    }
}
