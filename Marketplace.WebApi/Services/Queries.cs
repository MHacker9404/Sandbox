using System.Collections.Generic;
using System.Threading.Tasks;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.UserProfile;
using Marketplace.WebApi.Controllers.ClassifiedAds.QueryModels;
using Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Smuggler;

namespace Marketplace.WebApi.Services
{
    public static class Queries
    {
        public static Task<List<ClassifiedAdListItem>> Query(this IAsyncDocumentSession session, GetPublishedClassifiedAds query) =>
            session.Query<ClassifiedAd>().Where(ad => ad.State == ClassifiedAd.ClassifiedAdState.Active)
                   .Select(ad => new ClassifiedAdListItem
                                 {
                                     ClassifiedAdId = ad.Id.Value, Price = ad.Price.Amount
                                     , CurrencyCode = ad.Price.Currency.CurrencyCode
                                     , Title = ad.Title.Value
                                 }).PagedList(query.Page, query.PageSize);

        public static Task<List<ClassifiedAdListItem>> Query(this IAsyncDocumentSession session, GetOwnersClassifiedAds query) =>
            session.Query<ClassifiedAd>().Where(ad => ad.OwnerId.Value == query.OwnerId)
                   .Select(ad => new ClassifiedAdListItem
                                 {
                                     ClassifiedAdId = ad.Id.Value, Price = ad.Price.Amount
                                     , CurrencyCode = ad.Price.Currency.CurrencyCode
                                     , Title = ad.Title.Value
                                 }).PagedList(query.Page, query.PageSize);

        public static Task<ClassifiedAdDetails> Query(this IAsyncDocumentSession session, GetPublishedClassifiedAd query) =>
            (from ad in session.Query<ClassifiedAd>()
             where ad.Id.Value == query.ClassifiedAdId
             let user = RavenQuery.Load<UserProfile>($"UserProfile/{ad.OwnerId.Value}")
             select new ClassifiedAdDetails
                    {
                        ClassifiedAdId = ad.Id.Value, Title = ad.Title.Value, Description = ad.Text.Value, Price = ad.Price.Amount
                        , CurrencyCode = ad.Price.Currency.CurrencyCode, SellersDisplayName = user.DisplayName.Value
                    }).SingleAsync();

        private static Task<List<T>> PagedList<T>(this IRavenQueryable<T> query, int page, int pageSize) =>
            query.Skip(page * pageSize).Take(pageSize).ToListAsync();
    }
}