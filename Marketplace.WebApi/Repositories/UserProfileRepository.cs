using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Raven.Client.Documents.Session;

namespace Marketplace.WebApi.Repositories
{
    public class UserProfileRepository
        : RavenDbRepository<UserProfile, UserId>, IUserProfileRepository
    {
        public UserProfileRepository(IAsyncDocumentSession session) : base(session, id => $"UserProfile/{id.Value.ToString()}") { }
    }
}