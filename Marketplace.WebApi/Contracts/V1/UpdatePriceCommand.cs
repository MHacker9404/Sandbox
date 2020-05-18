using System;

namespace Marketplace.WebApi.Contracts.V1
{
    public class UpdatePriceCommand
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}