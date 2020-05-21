using System;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class PictureSize : Value<PictureSize>
    {
        private PictureSize() { }

        internal PictureSize(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public int Height { get; private set; }
        public int Width { get; private set; }

        public static PictureSize FromHeightWidth(int height, int width)
        {
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height), "Height must be a positive number");
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width), "Width must be a positive number");
            return new PictureSize(height, width);
        }
    }
}