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
        protected DisplayName() { }
        public string Value { get; }

        public static DisplayName FromString(string displayName, CheckTextForProfanity hasProfanity)
        {
            if (displayName.IsEmpty()) throw new ArgumentNullException(nameof(displayName));

            if (hasProfanity(displayName)) throw new ProfanityFoundException(nameof(displayName));
            return new DisplayName(displayName);
        }

        public static implicit operator string(DisplayName displayName) => displayName.Value;
    }
}