﻿using System;

namespace Marketplace.Domain.ClassifiedAd.Events
{
    public class PictureResized
    {
        public Guid PictureId { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}