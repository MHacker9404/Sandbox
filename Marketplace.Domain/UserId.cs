using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class UserId : Value<UserId>
    {
        public Guid Value { get; }

        internal UserId(Guid value) => Value = value;

        public static UserId FromGuid(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), $"User ID cannot be empty");
            }

            return new UserId(value);
        }

        public static implicit operator Guid(UserId self) => self.Value;
    }
}