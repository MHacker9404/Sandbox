using System;

namespace Marketplace.Domain.ClassifiedAd.Events
{
    public class ClassifiedAdCreated
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
    }
}
