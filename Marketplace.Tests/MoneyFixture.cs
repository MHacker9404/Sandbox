using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Marketplace.Domain;
using Marketplace.Domain.Shared;
using Marketplace.Domain.Shared.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Marketplace.Tests
{
    [TestClass]
    public class MoneyFixture
    {
        private readonly IEnumerable<CurrencyDetails> _currencies = new[]
                                                                    {
                                                                        new CurrencyDetails
                                                                        {
                                                                            CurrencyCode = "EUR", Decimals = 2 , InUse = true
                                                                        }
                                                                        , new CurrencyDetails
                                                                          {
                                                                              CurrencyCode = "USD", Decimals = 2 , InUse = true
                                                                          }
                                                                        , new CurrencyDetails
                                                                          {
                                                                              CurrencyCode = "JPY", Decimals = 0 , InUse = true
                                                                          }
                                                                        , new CurrencyDetails
                                                                          {
                                                                              CurrencyCode = "DEM", Decimals = 0 , InUse = true
                                                                          }
                                                                        ,
                                                                    };
        private readonly Mock<ICurrencyLookup> _mock = new Mock<ICurrencyLookup>();

        public MoneyFixture()
        {
            _mock.Setup(m => m.FindCurrency(It.IsAny<string>())).Returns((string code) =>
                                                                         {
                                                                             var detail = 
                                                                                 _currencies.FirstOrDefault(currency => currency.CurrencyCode == code);
                                                                             return detail ?? CurrencyDetails.None;
                                                                         });
        }

        [TestMethod]
        public void Two_of_same_amount_should_be_equal()
        {
            //  Arrange
            var first = Money.FromDecimal(5, "USD", _mock.Object);
            var second = Money.FromDecimal(5, "USD", _mock.Object);

            //  Act

            //  Assert
            first.Should().Be(second);
        }

        [TestMethod]
        public void Two_of_same_amount_different_types_should_not_be_equal()
        {
            //  Arrange
            var first = Money.FromDecimal(5, "EUR", _mock.Object);
            var second = Money.FromDecimal(5, "USD", _mock.Object);

            //  Act

            //  Assert
            first.Should().NotBe(second);
        }

        [TestMethod]
        public void FromString_and_FromDecimal_should_be_equal()
        {
            //  Arrange
            var first = Money.FromDecimal(5, "USD", _mock.Object);
            var second = Money.FromString("5", "USD", _mock.Object);

            //  Act

            //  Assert
            first.Should().Be(second);
        }

        [TestMethod]
        public void Sum_of_money_gives_full_amount()
        {
            //  Arrange
            var coin1 = Money.FromDecimal(1, "USD", _mock.Object);
            var coin2 = Money.FromDecimal(2, "USD", _mock.Object);
            var coin3 = Money.FromDecimal(2, "USD", _mock.Object);
            var note = Money.FromDecimal(5, "USD", _mock.Object);

            //  Act

            //  Assert
            note.Should().Be(coin1 + coin2 + coin3);
        }

        [TestMethod]
        public void Unused_currency_should_not_be_allowed()
        {
            //  Arrange
            Action action = () => Money.FromDecimal(10, "Nunya", _mock.Object);

            //  Act

            //  Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Too_many_decimal_places_should_not_be_allowed()
        {
            //  Arrange
            Action action = () => Money.FromDecimal(100.123m, "EUR", _mock.Object);

            //  Act

            //  Assert
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Adding_different_currencies_should_not_be_allowed()
        {
            //  Arrange
            var first = Money.FromDecimal(5, "USD", _mock.Object);
            var second = Money.FromDecimal(5, "EUR", _mock.Object);

            //  Act

            //  Assert
            FluentActions.Invoking(() => first + second).Should().Throw<CurrencyMismatchException>();
        }

        [TestMethod]
        public void Subtracting_different_currencies_should_not_be_allowed()
        {
            //  Arrange
            var first = Money.FromDecimal(5, "USD", _mock.Object);
            var second = Money.FromDecimal(5, "EUR", _mock.Object);

            //  Act

            //  Assert
            FluentActions.Invoking(() => first - second).Should().Throw<CurrencyMismatchException>();
        }
    }
}