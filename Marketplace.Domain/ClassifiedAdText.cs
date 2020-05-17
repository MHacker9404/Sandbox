using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        private ClassifiedAdText(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Text cannot be longer than 100 characters");
            }

            Value = value;
        }

        public string Value { get; }
        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);

        public static implicit operator string(ClassifiedAdText self) => self.Value;
    }
}