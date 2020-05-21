using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels
{
    public class ClassifiedAdListItem
    {
        public Guid ClassifiedAdId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string PhotoUrl { get; set; }
    }
}
