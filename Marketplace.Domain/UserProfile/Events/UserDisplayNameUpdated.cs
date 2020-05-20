using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain.UserProfile.Events
{
    public class UserDisplayNameUpdated
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
    }
}
