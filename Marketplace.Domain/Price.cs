using System;

namespace Marketplace.Domain
{
    public class Price : Money
    {
        private Price() { }

        private Price(decimal amount, string currency, ICurrencyLookup lookup) : base(amount, currency, lookup) { }

        public Price(decimal amount, string currencyCode)
            : base(amount, new CurrencyDetails {CurrencyCode = currencyCode}) { }

        public new static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup)
        {
            CheckValidity(amount);

            return new Price(amount, currency, currencyLookup);
        }

        public static implicit operator decimal(Price self) => self.Amount;

        private static void CheckValidity(decimal amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Price cannot be negative");
        }
    }
}