using System;
using System.Threading.Tasks;
using Marketplace.Framework;
using Raven.Client.Documents.Session;

namespace Marketplace.WebApi.Repositories
{
    public class RavenDbRepository<T, TId>
        where T : AggregateRoot<TId>
        where TId : Value<TId>
    {
        private readonly Func<TId, string> _entityId;
        private readonly IAsyncDocumentSession _session;

        public RavenDbRepository(IAsyncDocumentSession session, Func<TId, string> entityId)
        {
            _session = session;
            _entityId = entityId;
        }

        public async Task AddAsync(T entity) => await _session.StoreAsync(entity, _entityId(entity.Id));

        public async Task<bool> ExistsAsync(TId id) => await _session.Advanced.ExistsAsync(_entityId(id));

        public async Task<T> LoadAsync(TId id) => await _session.LoadAsync<T>(_entityId(id));
    }
}