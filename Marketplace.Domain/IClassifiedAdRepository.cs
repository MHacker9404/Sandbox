using System.Threading.Tasks;

namespace Marketplace.Domain
{
    public interface IClassifiedAdRepository
    {
        Task<bool> ExistsAsync(ClassifiedAdId id);

        Task<ClassifiedAd> LoadAsync(ClassifiedAdId id);

        Task AddAsync(ClassifiedAd entity);
    }
}