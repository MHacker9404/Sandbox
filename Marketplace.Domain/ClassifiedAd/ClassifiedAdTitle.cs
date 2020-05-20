using System;
using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        public ClassifiedAdTitle(string value) => Value = value;
        private ClassifiedAdTitle() { }

        public string Value { get; }

        public static ClassifiedAdTitle FromString(string title)
        {
            if (title.Length > 100) throw new ArgumentOutOfRangeException(nameof(title), "Title cannot be longer than 100 characters");

            return new ClassifiedAdTitle(title);
        }

        public static ClassifiedAdTitle FromHtml(string htmlTitle)
        {
            var tags = htmlTitle
                       .Replace("<i>", "")
                       .Replace("</i>", "")
                       .Replace("<b>", "")
                       .Replace("</b>", "");
            var value = Regex.Replace(tags, "<.*?>", string.Empty);
            CheckValidity(value);
            return new ClassifiedAdTitle(value);
        }

        public static implicit operator string(ClassifiedAdTitle self) => self.Value;

        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
                throw new ArgumentOutOfRangeException(
                    "Title cannot be longer that 100 characters"
                    , nameof(value));
        }
    }
}