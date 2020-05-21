using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Controllers.ClassifiedAds.QueryModels
{
    public class GetPublishedClassifiedAds
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
