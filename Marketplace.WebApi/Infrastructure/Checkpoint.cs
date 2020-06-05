using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Marketplace.WebApi.Infrastructure
{
    public class Checkpoint
    {
        public Position Position { get; set; }
        private string DbId
        {
            get => $"{GetType().Name}";
            set { }
        }
    }
}
