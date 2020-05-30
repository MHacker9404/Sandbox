using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Contracts.V1.ClassifiedAd
{
    public class PublishCommand
    {
        public Guid Id { get; set; }
        public Guid ApprovedBy { get; set; }
    }
}
