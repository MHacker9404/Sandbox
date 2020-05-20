using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;
using Marketplace.Domain.Shared;

namespace Marketplace.WebApi.Services
{
    public class FixedCurrencyLookup : ICurrencyLookup
    {
        private static readonly IEnumerable<CurrencyDetails> _currencies =
            new[]
            {
                new CurrencyDetails
                {
                    CurrencyCode = "EUR", Decimals = 2, InUse = true
                }
                , new CurrencyDetails
                  {
                      CurrencyCode = "USD", Decimals = 2, InUse = true
                  }
            };

        public CurrencyDetails FindCurrency(string currencyCode)
        {
            var currency = _currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode);
            return currency ?? CurrencyDetails.None;
        }
    }
}