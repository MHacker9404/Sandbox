using System;
using System.Collections.Generic;
using System.Text;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class PictureId : Value<PictureId>
    {
        public Guid Value { get; }

        internal PictureId(Guid value) => Value = value;

        public static PictureId FromGuid(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), $"PictureId cannot be empty");
            }

            return new PictureId(value);
        }

        public static implicit operator Guid(PictureId self) => self.Value;
    }
}
