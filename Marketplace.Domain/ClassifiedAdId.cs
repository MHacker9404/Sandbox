using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        public Guid Value { get; }

        internal ClassifiedAdId(Guid value) => Value = value;

        public static ClassifiedAdId FromGuid(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), $"ClassifiedAd ID cannot be empty");
            }

            return new ClassifiedAdId(value);
        }

        public static implicit operator Guid(ClassifiedAdId self) => self.Value;
    }
}
