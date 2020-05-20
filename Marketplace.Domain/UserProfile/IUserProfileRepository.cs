using System.Threading.Tasks;
using Marketplace.Domain.Shared;

namespace Marketplace.Domain.UserProfile
{
    public interface IUserProfileRepository
    {
        Task<bool> ExistsAsync(UserId id);

        Task<UserProfile> LoadAsync(UserId id);

        Task AddAsync(UserProfile entity);
    }
}