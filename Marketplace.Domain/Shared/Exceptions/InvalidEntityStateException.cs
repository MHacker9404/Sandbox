using System;

namespace Marketplace.Domain.Shared.Exceptions
{
    public class InvalidEntityStateException : Exception
    {
        public InvalidEntityStateException(object entity, string message) : base($"Entity {entity.GetType()} state change rejected, {message}") { }
    }
}