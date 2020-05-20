using Marketplace.Domain.ClassifiedAd;
using Raven.Client.Documents.Session;

namespace Marketplace.WebApi.Repositories
{
    public class ClassifiedAdRepository
        : RavenDbRepository<ClassifiedAd, ClassifiedAdId>, IClassifiedAdRepository
    {
        public ClassifiedAdRepository(IAsyncDocumentSession session) : base(session, id => $"ClassifiedAd/{id.Value.ToString()}") { }

        ////  this is the UnitOfWork
        //private readonly IAsyncDocumentSession _session;

        //public ClassifiedAdRepository(IAsyncDocumentSession session) => _session = session;

        //public async Task<bool> ExistsAsync(ClassifiedAdId id) => await _session.Advanced.ExistsAsync(EntityId(id));

        //public async Task<ClassifiedAd> LoadAsync(ClassifiedAdId id) => await _session.LoadAsync<ClassifiedAd>(EntityId(id));

        //public async Task AddAsync(ClassifiedAd entity) => await _session.StoreAsync(entity, EntityId(entity.Id));

        //public void Dispose() => _session.Dispose();

        //private static string EntityId(ClassifiedAdId id) => $"ClassifiedAd/{id.ToString()}";
    }
}