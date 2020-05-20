using System;

namespace Marketplace.Domain.Shared.Exceptions
{
    public class ProfanityFoundException : Exception
    {
        public ProfanityFoundException(string displayName) : base($"Profanity found in text: {displayName}") { }

        public ProfanityFoundException() { }

        public ProfanityFoundException(string displayName, Exception innerException) : base( $"Profanity found in text: {displayName}", innerException) { }
    }
}