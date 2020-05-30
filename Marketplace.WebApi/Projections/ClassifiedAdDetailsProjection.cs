using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.Domain.ClassifiedAd.Events;
using Marketplace.Domain.UserProfile.Events;
using Marketplace.Framework;
using Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels;
using Serilog;
using ClassifiedAdPublished = Marketplace.WebApi.Projections.Upcasts.ClassifiedAdPublished;

namespace Marketplace.WebApi.Projections
{
    public class ClassifiedAdDetailsProjection : IProjection
    {
        private readonly HashSet<ClassifiedAdDetails> _details;
        private readonly Func<Guid, string> _getUserDisployName;
        private readonly ILogger _logger;

        public ClassifiedAdDetailsProjection(HashSet<ClassifiedAdDetails> details, ILogger logger, Func<Guid, string> getUserDisployName)
        {
            _details = details;
            _logger = logger;
            _getUserDisployName = getUserDisployName;
        }

        public Task Project(object evt)
        {
            _logger.Debug($"Projecting event {evt.GetType().Name}");
            switch (evt)
            {
                case ClassifiedAdCreated e:
                    if (_details.SingleOrDefault(detail => detail.ClassifiedAdId == e.Id) == null)
                        _details.Add(new ClassifiedAdDetails
                                     {
                                         ClassifiedAdId = e.Id, SellerId = e.OwnerId, SellersDisplayName = _getUserDisployName(e.OwnerId)
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

                case ClassifiedAdPublished e:
                    UpdateItem(e.Id, ad => ad.SellersPhotoUrl = e.SellersPhotoUrl);
                    break;

                case UserDisplayNameUpdated e:
                    UpdateMultipleItems(ad => ad.SellerId == e.UserId, ad => ad.SellersDisplayName = e.DisplayName);
                    break;
            }

            return Task.CompletedTask;
        }

        private void UpdateMultipleItems(Func<ClassifiedAdDetails, bool> pred, Action<ClassifiedAdDetails> action)
        {
            foreach (var detail in _details.Where(pred)) action(detail);
        }

        private void UpdateItem(Guid id, Action<ClassifiedAdDetails> action)
        {
            var detail = _details.SingleOrDefault(d => d.ClassifiedAdId == id);
            if (detail != null) action(detail);
        }
    }
}