using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.Domain.ClassifiedAd.Events;
using Marketplace.Domain.UserProfile.Events;
using Marketplace.Framework;
using Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Marketplace.WebApi.Infrastructure
{
    public class ProjectionManager
    {
        private readonly IEventStoreConnection _esConnection;
        private readonly IProjection[] _projections;
        private readonly ILogger _logger;
        private EventStoreAllCatchUpSubscription _subscription;

        public ProjectionManager(IEventStoreConnection esConnection, IProjection[] projections, ILogger logger)
        {
            _esConnection = esConnection;
            _projections = projections;
            _logger = logger;
        }

        public void Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000
                                                           , 500
                                                           , _logger.IsEnabled(LogEventLevel.Verbose)
                                                           , true
                                                           , "try-out-subscription");
            _subscription = _esConnection.SubscribeToAllFrom(Position.Start, settings, EventAppeared);
        }

        public void Stop() => _subscription.Stop();

        private Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$"))
                return Task.CompletedTask;

            var evt = resolvedEvent.Deserialize();
            _logger.Debug($"Projecting event {evt.GetType().Name}: {resolvedEvent.Event.EventId}");

            return Task.WhenAll(_projections.Select(p => p.Project(evt)));
        }
    }
}