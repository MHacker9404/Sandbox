using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain.UserProfile.Events
{
    public class UserFullNameUpdated
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
    }
}
