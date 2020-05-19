using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.Domain;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.WebApi.Repositories
{
    public class EfRepositoryImpl
        : IClassifiedAdRepository
    {
        private readonly ClassifiedAdDbContext _context;

        public EfRepositoryImpl(ClassifiedAdDbContext context) => _context = context;
        public async Task<bool> ExistsAsync(ClassifiedAdId id) => await _context.ClassifiedAds.FirstOrDefaultAsync(ad => ad.AdId == id.Value) != null;

        public async Task<ClassifiedAd> LoadAsync(ClassifiedAdId id) => await _context.ClassifiedAds.SingleAsync(ad => ad.AdId == id.Value);

        public async Task AddAsync(ClassifiedAd entity) => await _context.ClassifiedAds.AddAsync(entity);
    }
}
