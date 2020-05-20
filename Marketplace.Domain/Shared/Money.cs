using System;
using Marketplace.Domain.Shared.Exceptions;
using Marketplace.Framework;

namespace Marketplace.Domain.Shared
{
    public class Money : Value<Money>
    {
        private const string DefaultCurrency = "USD";

        protected Money() { }

        protected Money(decimal amount, string currency, ICurrencyLookup currencyLookup)
        {
            if (string.IsNullOrEmpty(currency)) throw new ArgumentNullException(nameof(currency), "Currency must be specified");

            var details = currencyLookup.FindCurrency(currency);
            if (!details.InUse) throw new ArgumentException($"Currency {currency} is not valid");

            if (decimal.Round(amount, details.Decimals) != amount)
                throw new ArgumentOutOfRangeException(nameof(amount)
                                                      , $"Amount in {currency} cannot have more than {details.Decimals} decimals");

            Amount = amount;
            Currency = details;
        }

        protected Money(decimal amount, CurrencyDetails currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public CurrencyDetails Currency { get; }

        public decimal Amount { get; }

        public Money Add(Money right)
        {
            if (Currency != right.Currency) throw new CurrencyMismatchException("Cannot sum amounts with different currencies");

            return new Money(Amount + right.Amount, Currency);
        }

        public Money Subtract(Money right)
        {
            if (Currency != right.Currency) throw new CurrencyMismatchException("Cannot subtract amounts with different currencies");

            return new Money(Amount - right.Amount, Currency);
        }

        public static Money operator +(Money left, Money right) => left.Add(right);
        public static Money operator -(Money left, Money right) => left.Subtract(right);

        public static Money FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup) => new Money(amount, currency, currencyLookup);

        public static Money FromString(string amount, string currency, ICurrencyLookup currencyLookup) =>
            new Money(decimal.Parse(amount), currency, currencyLookup);

        public override string ToString() => $"{Currency.CurrencyCode} {Amount}";

        //public bool Equals(Money other)
        //{
        //    if (ReferenceEquals(null, other)) return false;
        //    if (ReferenceEquals(this, other)) return true;
        //    return Amount == other.Amount;
        //}

        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != this.GetType()) return false;

        //    return Equals((Money) obj);
        //}

        //public override int GetHashCode() => Amount.GetHashCode();

        //public static bool operator ==(Money left, Money right) => Equals(left, right);

        //public static bool operator !=(Money left, Money right) => !Equals(left, right);
    }
}