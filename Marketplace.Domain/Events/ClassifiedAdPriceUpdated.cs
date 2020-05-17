using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain.Events
{
    public class ClassifiedAdPriceUpdated
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
