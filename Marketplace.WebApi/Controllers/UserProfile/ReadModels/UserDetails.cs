using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Controllers.UserProfile.ReadModels
{
    public class UserDetails
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        private string DbId
        {
            get => $"{GetType().Name}/{Id}";
            set { }
        }
    }
}
