using System;
using System.Threading.Tasks;
using Marketplace.Domain.ClassifiedAd.Events;
using Marketplace.Domain.UserProfile.Events;
using Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels;
using Raven.Client.Documents.Session;
using Serilog;
using ClassifiedAdPublished = Marketplace.WebApi.Projections.Upcasts.ClassifiedAdPublished;

namespace Marketplace.WebApi.Projections
{
    public class ClassifiedAdDetailsProjection : RavenDbProjection<ClassifiedAdDetails>
    {
        private readonly Func<Guid, Task<string>> _getUserDisployName;
        private readonly ILogger _logger;

        public ClassifiedAdDetailsProjection(Func<IAsyncDocumentSession> getSession, ILogger logger, Func<Guid, Task<string>> getUserDisployName)
            : base(getSession, logger)
        {
            _logger = logger;
            _getUserDisployName = getUserDisployName;
        }

        public override Task Project(object evt)
        {
            _logger.Debug($"Projecting event {evt.GetType().Name}");
            return evt switch
                   {
                       ClassifiedAdCreated e =>
                       Create(async () =>
                                  new ClassifiedAdDetails
                                  {
                                      Id = e.Id.ToString(), ClassifiedAdId = e.Id, SellerId = e.OwnerId
                                      , SellersDisplayName = await _getUserDisployName(e.OwnerId)
                                  })
                       , ClassifiedAdTitleChanged e => UpdateOne(e.Id, ad => ad.Title = e.Title)
                       , ClassifiedAdTextUpdated e => UpdateOne(e.Id, ad => ad.Description = e.Text)
                       , ClassifiedAdPriceUpdated e => UpdateOne(e.Id
                                                                 , ad =>
                                                                   {
                                                                       ad.Price = e.Price;
                                                                       ad.CurrencyCode = e.Currency;
                                                                   })
                       , UserDisplayNameUpdated e => UpdateWhere(user => user.SellerId == e.UserId
                                                                 , ad => ad.SellersDisplayName = e.DisplayName)
                       , ClassifiedAdPublished e => UpdateOne(e.Id, ad => ad.SellersPhotoUrl = e.SellersPhotoUrl)
                       , _ => Task.CompletedTask
                   };
        }
    }
}