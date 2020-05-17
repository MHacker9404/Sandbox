using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        internal ClassifiedAdText(string value) => Value = value;

        public string Value { get; }

        public static ClassifiedAdText FromString(string text)
        {
            if (text.Length > 100) throw new ArgumentOutOfRangeException(nameof(text), "Text cannot be longer than 100 characters");

            return new ClassifiedAdText(text);
        }

        public static implicit operator string(ClassifiedAdText self) => self.Value;
    }
}