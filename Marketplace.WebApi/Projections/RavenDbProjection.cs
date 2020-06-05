using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marketplace.Framework;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Serilog;

namespace Marketplace.WebApi.Projections
{
    public abstract class RavenDbProjection<T> : IProjection
    {
        private readonly Func<IAsyncDocumentSession> _getSession;
        protected readonly ILogger _logger;

        protected RavenDbProjection(Func<IAsyncDocumentSession> getSession, ILogger logger)
        {
            _getSession = getSession;
            _logger = logger;
        }

        public abstract Task Project(object @event);
        protected Task Create(Func<Task<T>> func) => UsingSession(async session => await session.StoreAsync(await func()));
        protected Task UpdateOne(Guid id, Action<T> action) => UsingSession(session => UpdateItem(session, id, action));

        protected Task UpdateWhere(Expression<Func<T, bool>> where, Action<T> action) =>
            UsingSession(session => UpdateMultipleItems(session, where, action));

        private static async Task UpdateMultipleItems(IAsyncDocumentSession session, Expression<Func<T, bool>> query, Action<T> action)
        {
            var items = await session.Query<T>().Where(query).ToListAsync();
            items.ForEach(action);
        }

        private static async Task UpdateItem(IAsyncDocumentSession session, Guid id, Action<T> action)
        {
            var item = await session.LoadAsync<T>(id.ToString());
            if (item == null) return;
            action(item);
        }

        private async Task UsingSession(Func<IAsyncDocumentSession, Task> op)
        {
            using var session = _getSession();
            await op(session);
            await session.SaveChangesAsync();
        }
    }
}