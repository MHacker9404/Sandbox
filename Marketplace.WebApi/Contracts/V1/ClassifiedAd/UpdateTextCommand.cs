using System;

namespace Marketplace.WebApi.Contracts.V1.ClassifiedAd
{
    public class UpdateTextCommand
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}