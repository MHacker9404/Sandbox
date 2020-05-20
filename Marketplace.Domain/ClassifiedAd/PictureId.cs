using System;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class PictureId : Value<PictureId>
    {
        public PictureId(Guid value) => Value = value;

        private PictureId() { }

        public Guid Value { get; }

        public static PictureId FromGuid(Guid value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value), "PictureId cannot be empty");

            return new PictureId(value);
        }

        public static implicit operator Guid(PictureId self) => self.Value;
    }
}