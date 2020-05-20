using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public interface IClassifiedAdRepository
    {
        Task<bool> ExistsAsync(ClassifiedAdId id);

        Task<ClassifiedAd> LoadAsync(ClassifiedAdId id);

        Task AddAsync(ClassifiedAd entity);
    }
}