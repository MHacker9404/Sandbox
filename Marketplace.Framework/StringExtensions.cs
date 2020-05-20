using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Framework
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);
    }
}
