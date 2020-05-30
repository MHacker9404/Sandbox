using System;

namespace Marketplace.Domain.ClassifiedAd.Events
{
    public class ClassifiedAdPublished
    {
        public Guid Id { get; set; }
        public Guid ApprovedBy { get; set; }
        public Guid OwnerId { get; set; }
    }
}