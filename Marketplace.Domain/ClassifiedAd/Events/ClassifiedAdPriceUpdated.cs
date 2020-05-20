using System;

namespace Marketplace.Domain.ClassifiedAd.Events
{
    public class ClassifiedAdPriceUpdated
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
