using System;

namespace Marketplace.WebApi.Projections.Upcasts
{
    public class ClassifiedAdPublished
    {
        public Guid Id { get; set; }
        public Guid ApprovedBy { get; set; }
        public Guid OwnerId { get; set; }
        public string SellersPhotoUrl { get; set; }
    }
}