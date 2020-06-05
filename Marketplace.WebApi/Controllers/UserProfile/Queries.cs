using System;
using System.Threading.Tasks;
using Marketplace.WebApi.Controllers.UserProfile.ReadModels;
using Raven.Client.Documents.Session;

namespace Marketplace.WebApi.Controllers.UserProfile
{
    public static class Queries
    {
        public static async Task<UserDetails> GetUserDetailsAsync(this Func<IAsyncDocumentSession> getSession, Guid id)
        {
            using var session = getSession();
            var details = await session.LoadAsync<UserDetails>(id.ToString());
            return details;
        }
    }
}