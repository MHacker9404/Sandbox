using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Controllers.UserProfile.ReadModels
{
    public class UserDetails
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
    }
}
