using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain.UserProfile.Events
{
    public class ProfilePhotoUploaded
    {
        public Guid UserId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
