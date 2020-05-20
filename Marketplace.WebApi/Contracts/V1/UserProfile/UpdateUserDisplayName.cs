using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Contracts.V1.UserProfile
{
    public class UpdateUserDisplayName
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
    }
}
