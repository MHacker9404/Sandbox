using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.Domain.ClassifiedAd.Events;
using Marketplace.Domain.UserProfile.Events;
using Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Marketplace.WebApi.Infrastructure
{
    public class EaSubscription
    {
        private readonly IEventStoreConnection _esConnection;
        private readonly IList<ClassifiedAdDetails> _items;
        private readonly ILogger _logger;
        private EventStoreAllCatchUpSubscription _subscription;

        public EaSubscription(IEventStoreConnection esConnection, IList<ClassifiedAdDetails> items, ILogger logger)
        {
            _esConnection = esConnection;
            _items = items;
            _logger = logger;
        }

        public void Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000
                                                           , 500
                                                           , _logger.IsEnabled(LogEventLevel.Verbose)
                                                           , false
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
            switch (evt)
            {
                case ClassifiedAdCreated e:
                    _items.Add(new ClassifiedAdDetails
                               {
                                   ClassifiedAdId = e.Id, SellerId = e.OwnerId
                               });
                    break;

                case ClassifiedAdTitleChanged e:
                    UpdateItem(e.Id, ad => ad.Title = e.Title);
                    break;

                case ClassifiedAdTextUpdated e:
                    UpdateItem(e.Id, ad => ad.Description = e.Text);
                    break;

                case ClassifiedAdPriceUpdated e:
                    UpdateItem(e.Id
                               , ad =>
                                 {
                                     ad.Price = e.Price;
                                     ad.CurrencyCode = e.Currency;
                                 });
                    break;

                case UserDisplayNameUpdated e:
                    UpdateMultipleItems(ad => ad.SellerId == e.UserId, ad => ad.SellersDisplayName = e.DisplayName);
                    break;
            }

            return Task.CompletedTask;
        }

        private void UpdateMultipleItems(Func<ClassifiedAdDetails, bool> pred, Action<ClassifiedAdDetails> action)
        {
            foreach (var item in _items.Where(pred)) action(item);
        }

        private void UpdateItem(Guid id, Action<ClassifiedAdDetails> action)
        {
            var item = _items.FirstOrDefault(ad => ad.ClassifiedAdId == id);
            if (item == null) return;
            action(item);
        }
    }
}