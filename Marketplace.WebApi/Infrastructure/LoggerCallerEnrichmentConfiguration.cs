using Serilog;
using Serilog.Configuration;

namespace Marketplace.WebApi.Infrastructure
{
    internal static class LoggerCallerEnrichmentConfiguration
    {
        internal static LoggerConfiguration WithCaller(this LoggerEnrichmentConfiguration enrichmentConfiguration) =>
            enrichmentConfiguration.With<CallerEnricher>();
    }
}