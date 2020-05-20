using System;
using Marketplace.Framework;

namespace Marketplace.Domain.Shared
{
    public class UserId : Value<UserId>
    {
        private UserId() { }

        public UserId(Guid value) => Value = value;

        public Guid Value { get; private set; }

        public static UserId FromGuid(Guid value)
        {
            if (value == default) throw new ArgumentNullException(nameof(value), "User ID cannot be empty");

            return new UserId(value);
        }

        public static implicit operator Guid(UserId self) => self.Value;
    }
}