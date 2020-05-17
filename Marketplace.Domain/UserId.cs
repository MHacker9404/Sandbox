﻿using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class UserId : Value<UserId>
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), $"User ID cannot be empty");
            }

            Value = value;
        }

        public static implicit operator Guid(UserId self) => self.Value;
    }
}