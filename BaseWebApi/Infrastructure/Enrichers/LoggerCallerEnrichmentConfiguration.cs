using Serilog;
using Serilog.Configuration;

namespace BaseWebApi.Infrastructure.Enrichers
{
    internal static class LoggerCallerEnrichmentConfiguration
    {
        internal static LoggerConfiguration WithCaller(this LoggerEnrichmentConfiguration enrichmentConfiguration) =>
            enrichmentConfiguration.With<CallerEnricher>();
    }
}