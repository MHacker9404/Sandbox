using System;
using System.Threading.Tasks;
using Marketplace.Domain.UserProfile.Events;
using Marketplace.WebApi.Controllers.UserProfile.ReadModels;
using Raven.Client.Documents.Session;
using Serilog;

namespace Marketplace.WebApi.Projections
{
    public class UserDetailsProjection : RavenDbProjection<UserDetails>
    {
        public UserDetailsProjection(Func<IAsyncDocumentSession> getSession, ILogger logger) : base(getSession, logger) { }

        public override Task Project(object evt)
        {
            _logger.Debug($"Projecting event {evt.GetType().Name}");
            return evt switch
                   {
                       UserRegistered e => Create(async () => new UserDetails
                                                              {
                                                                  Id = e.UserId.ToString()
                                                                  ,UserId = e.UserId
                                                                  , DisplayName = e.DisplayName
                                                                  , FullName = e.FullName
                                                              })
                       , UserDisplayNameUpdated e => UpdateOne(e.UserId, user => user.DisplayName = e.DisplayName)
                       , ProfilePhotoUploaded e => UpdateOne(e.UserId, user => user.PhotoUrl = e.PhotoUrl)
                       , _ => Task.CompletedTask
                   };
        }

        //private void UpdateItem(Guid id, Action<UserDetails> action)
        //{
        //    var user = _users.SingleOrDefault(u => u.UserId == id);
        //    if (user != null) action(user);
        //}

        //private void UpdateItem(Guid eUserId, Func<object, object> func)
        //{
        //    throw new NotImplementedException();
        //}
    }
}