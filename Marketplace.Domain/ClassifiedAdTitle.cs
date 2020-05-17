using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        private ClassifiedAdTitle(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Title cannot be longer than 100 characters");
            }

            Value = value;
        }

        public string Value { get; }
        public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title);

        public static implicit operator string(ClassifiedAdTitle self) => self.Value;
    }
}