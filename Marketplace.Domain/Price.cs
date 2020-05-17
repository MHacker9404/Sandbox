using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain
{
    public class Price : Money
    {
        private Price(decimal amount, string currency, ICurrencyLookup lookup) : base(amount, currency, lookup)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), $"Price cannot be negative");
            }
        }

        public new static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup) =>
            new Price(amount, currency, currencyLookup);

        public static implicit operator decimal(Price self) => self.Amount;
    }
}
