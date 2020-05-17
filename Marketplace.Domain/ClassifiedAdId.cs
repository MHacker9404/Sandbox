using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        public Guid Value { get; }

        public ClassifiedAdId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), $"ClassifiedAd ID cannot be empty");
            }
        }

        public static implicit operator Guid(ClassifiedAdId self) => self.Value;
    }
}
