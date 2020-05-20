using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain.UserProfile.Events
{
    public class UserRegistered
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
    }
}
