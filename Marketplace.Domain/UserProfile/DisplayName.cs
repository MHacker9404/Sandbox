using System;
using System.Threading.Tasks;
using Marketplace.Domain.Shared;
using Marketplace.Domain.Shared.Exceptions;
using Marketplace.Framework;

namespace Marketplace.Domain.UserProfile
{
    public class DisplayName : Value<DisplayName>
    {
        internal DisplayName(string displayName) => Value = displayName;

        //  serialization
        private DisplayName() { }
        public string Value { get; private set; }

        public static DisplayName FromString(string displayName, CheckTextForProfanity hasProfanity)
        {
            if (displayName.IsEmpty()) throw new ArgumentNullException(nameof(displayName));

            if (hasProfanity(displayName).GetAwaiter().GetResult()) throw new ProfanityFoundException(nameof(displayName));
            return new DisplayName(displayName);
        }

        public static implicit operator string(DisplayName displayName) => displayName?.Value;
    }
}