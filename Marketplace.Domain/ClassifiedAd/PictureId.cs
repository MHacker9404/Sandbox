using System;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class PictureId : Value<PictureId>
    {
        private PictureId() { }
        internal PictureId(Guid value) => Value = value;

        public Guid Value { get; private set; }

        public static PictureId FromGuid(Guid value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value), "PictureId cannot be empty");

            return new PictureId(value);
        }

        public static implicit operator Guid(PictureId self) => self.Value;
    }
}