using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.Framework;

namespace Marketplace.WebApi.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ClassifiedAdDbContext _context;

        public EfUnitOfWork(ClassifiedAdDbContext context) => _context = context;
        public Task CommitAsync() => _context.SaveChangesAsync();
    }
}
