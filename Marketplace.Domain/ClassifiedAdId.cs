using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        public ClassifiedAdId(Guid value) => Value = value;

        public Guid Value { get; private set; }

        private ClassifiedAdId() { }

        public static ClassifiedAdId FromGuid(Guid value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value), "ClassifiedAd ID cannot be empty");

            return new ClassifiedAdId(value);
        }

        public static implicit operator Guid(ClassifiedAdId self) => self.Value;
    }
}