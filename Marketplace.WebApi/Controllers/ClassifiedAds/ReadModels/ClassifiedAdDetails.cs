using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels
{
    public class ClassifiedAdDetails
    {
        public Guid ClassifiedAdId { get; set; }
        public Guid SellerId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string SellersDisplayName { get; set; }
        public string SellersPhotoUrl { get; set; }
        public string[] PhotoUrls { get; set; }
    }
}
