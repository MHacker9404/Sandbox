namespace Marketplace.Domain.ClassifiedAd
{
    public static class PictureRules
    {
        public static bool HasCorrectSize(this Picture picture) => picture != null && picture.Size.Height >= 800 && picture.Size.Width >= 600;
    }
}