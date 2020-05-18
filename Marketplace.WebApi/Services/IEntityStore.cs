using System.Threading.Tasks;
using Marketplace.Domain;

namespace Marketplace.WebApi.Services {
    public interface IEntityStore {
        Task SaveAsync(ClassifiedAd classifiedAd);
    }
}