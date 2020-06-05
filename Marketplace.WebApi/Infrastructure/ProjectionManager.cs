using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly ICheckpointStore _checkpointStore;
        private EventStoreAllCatchUpSubscription _subscription;

        public ProjectionManager(IEventStoreConnection esConnection, IProjection[] projections, ILogger logger, ICheckpointStore checkpointStore)
        {
            _esConnection = esConnection;
            _projections = projections;
            _logger = logger;
            _checkpointStore = checkpointStore;
        }

        public async Task Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000
                                                           , 500
                                                           , _logger.IsEnabled(LogEventLevel.Verbose)
                                                           , false
                                                           , "try-out-subscription");
            var position = await _checkpointStore.GetCheckpointAsync();

            _subscription = _esConnection.SubscribeToAllFrom(position, settings, EventAppeared);
        }

        public void Stop() => _subscription.Stop();

        private async Task EventAppeared(EventStoreCatchUpSubscription subscription,  ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$"))
                return;

            var evt = resolvedEvent.Deserialize();
            _logger.Debug($"Projecting event {evt.GetType().Name}: {resolvedEvent.Event.EventId}");

            await Task.WhenAll(_projections.Select(p => p.Project(evt)));
            await _checkpointStore.StoreCheckpointAsync(resolvedEvent.OriginalPosition.Value);
        }
    }
}