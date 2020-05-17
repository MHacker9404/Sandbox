using System;
using System.Collections.Generic;
using System.Text;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class CurrencyDetails : Value<CurrencyDetails>
    {
        public string CurrencyCode { get; set; }
        public bool InUse { get; set; }
        public int Decimals { get; set; }

        public static CurrencyDetails None = new CurrencyDetails {InUse = false};

        public static implicit operator string(CurrencyDetails self) => self.CurrencyCode;
    }
}