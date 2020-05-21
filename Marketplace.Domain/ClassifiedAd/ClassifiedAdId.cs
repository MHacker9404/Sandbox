using System;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        private ClassifiedAdId() { }
        internal ClassifiedAdId(Guid value) => Value = value;

        public Guid Value { get; private set; }

        public static ClassifiedAdId FromGuid(Guid value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value), "ClassifiedAd ID cannot be empty");

            return new ClassifiedAdId(value);
        }

        public static implicit operator Guid(ClassifiedAdId self) => self.Value;
    }
}