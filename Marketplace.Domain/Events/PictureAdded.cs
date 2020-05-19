using System;

namespace Marketplace.Domain.Events
{
    public class PictureAdded
    {
        public Guid Id { get; set; }
        public Guid ClassifiedAdId { get; set; }
        public string Url { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Order { get; set; }
    }
}