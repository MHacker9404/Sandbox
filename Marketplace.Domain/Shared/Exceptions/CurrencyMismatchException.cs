using System;

namespace Marketplace.Domain.Shared.Exceptions {
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException( string message ) : base( message )
        {
        }
    }
}