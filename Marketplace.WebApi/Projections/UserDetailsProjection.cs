using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketplace.Domain.UserProfile.Events;
using Marketplace.Framework;
using Marketplace.WebApi.Controllers.UserProfile.ReadModels;
using Serilog;

namespace Marketplace.WebApi.Projections
{
    public class UserDetailsProjection : IProjection
    {
        private readonly ILogger _logger;
        private readonly HashSet<UserDetails> _users;

        public UserDetailsProjection( HashSet<UserDetails> users, ILogger logger )
        {
            _users = users;
            _logger = logger;
        }

        public Task Project( object evt )
        {
            _logger.Debug( $"Projecting event {evt.GetType( ).Name}" );
            switch ( evt )
            {
                case UserRegistered e:
                    if ( _users.SingleOrDefault( user => user.UserId == e.UserId ) == null )
                    {
                        _users.Add( new UserDetails
                        {
                            UserId = e.UserId,
                            DisplayName = e.DisplayName
                        } );
                    }
                    break;

                case UserDisplayNameUpdated e:
                    UpdateItem( e.UserId, user => user.DisplayName = e.DisplayName );
                    break;
            }

            return Task.CompletedTask;
        }

        private void UpdateItem( Guid id, Action<UserDetails> action )
        {
            var user = _users.SingleOrDefault(u => u.UserId == id);
            if (user != null)
            {
                action(user);
            }
        }

        private void UpdateItem( Guid eUserId, Func<object, object> func )
        {
            throw new NotImplementedException( );
        }
    }
}