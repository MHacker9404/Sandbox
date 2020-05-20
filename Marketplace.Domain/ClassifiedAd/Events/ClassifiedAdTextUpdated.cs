using System;

namespace Marketplace.Domain.ClassifiedAd.Events
{
    public class ClassifiedAdTextUpdated
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
