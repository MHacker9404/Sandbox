using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Contracts.V1.UserProfile
{
    public class UpdateUserProfilePhoto
    {
        public Guid UserId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
