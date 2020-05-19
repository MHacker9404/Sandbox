using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.Framework;
using Raven.Client.Documents.Session;

namespace Marketplace.WebApi.Repositories
{
    public class RavenDbUnitOfWork : IUnitOfWork
    {
        //  this is the UnitOfWork
        private readonly IAsyncDocumentSession _session;

        public RavenDbUnitOfWork(IAsyncDocumentSession session) => _session = session;

        public async Task CommitAsync() => await _session.SaveChangesAsync();
    }
}
