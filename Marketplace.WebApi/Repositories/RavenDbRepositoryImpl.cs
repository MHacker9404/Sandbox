using System;
using System.Threading.Tasks;
using Marketplace.Domain;
using Raven.Client.Documents.Session;

namespace Marketplace.WebApi.Repositories
{
    public class RavenDbRepositoryImpl
        : IClassifiedAdRepository, IDisposable
    {
        //  this is the UnitOfWork
        private readonly IAsyncDocumentSession _session;

        public RavenDbRepositoryImpl(IAsyncDocumentSession session) => _session = session;

        public async Task<bool> ExistsAsync(ClassifiedAdId id) => await _session.Advanced.ExistsAsync(EntityId(id));

        public async Task<ClassifiedAd> LoadAsync(ClassifiedAdId id) => await _session.LoadAsync<ClassifiedAd>(EntityId(id));

        public async Task AddAsync(ClassifiedAd entity) => await _session.StoreAsync(entity, EntityId(entity.Id));

        public void Dispose() => _session.Dispose();

        private static string EntityId(ClassifiedAdId id) => $"ClassifiedAd/{id.ToString()}";
    }
}