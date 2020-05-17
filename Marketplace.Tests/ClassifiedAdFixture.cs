using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Marketplace.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Marketplace.Tests
{
    [TestClass]
    public class ClassifiedAdFixture
    {
        private ClassifiedAd _sut;

        private readonly IEnumerable<CurrencyDetails> _currencies = new[]
                                                                    {
                                                                        new CurrencyDetails
                                                                        {
                                                                            CurrencyCode = "EUR", Decimals = 2, InUse = true
                                                                        }
                                                                        , new CurrencyDetails
                                                                          {
                                                                              CurrencyCode = "USD", Decimals = 2, InUse = true
                                                                          }
                                                                        , new CurrencyDetails
                                                                          {
                                                                              CurrencyCode = "JPY", Decimals = 0, InUse = true
                                                                          }
                                                                        , new CurrencyDetails
                                                                          {
                                                                              CurrencyCode = "DEM", Decimals = 0, InUse = true
                                                                          }
                                                                        ,
                                                                    };

        private readonly Mock<ICurrencyLookup> _mock = new Mock<ICurrencyLookup>();

        [TestInitialize]
        public void TestSetup()
        {
            _sut = new ClassifiedAd(ClassifiedAdId.FromGuid(Guid.NewGuid()), UserId.FromGuid(Guid.NewGuid()));
        }

        public ClassifiedAdFixture()
        {
            _mock.Setup(m => m.FindCurrency(It.IsAny<string>())).Returns((string code) =>
                                                                         {
                                                                             var detail =
                                                                                 _currencies.FirstOrDefault(currency => currency.CurrencyCode == code);
                                                                             return detail ?? CurrencyDetails.None;
                                                                         });
        }

        [TestMethod]
        public void Can_publish_a_valid_ad()
        {
            //  Arrange
            _sut.SetTitle( ClassifiedAdTitle.FromString( "Title" ) );
            _sut.UpdateText( ClassifiedAdText.FromString( "Text" ) );
            _sut.UpdatePrice(Price.FromDecimal(100.10m, "USD", _mock.Object));

            //  Act
            _sut.RequestToPublish();

            //  Assert
            _sut.State.Should().Be(ClassifiedAd.ClassifiedAdState.PendingReview);
        }

        [TestMethod]
        public void Cannot_publish_without_title()
        {
            //  Arrange
            _sut.UpdateText(ClassifiedAdText.FromString("Text"));
            _sut.UpdatePrice(Price.FromDecimal(100.10m, "USD", _mock.Object));

            //  Act
            Action action = () => _sut.RequestToPublish();

            //  Assert
            action.Should().Throw<InvalidEntityStateException>();
        }

        [TestMethod]
        public void Cannot_publish_without_text()
        {
            //  Arrange
            _sut.SetTitle(ClassifiedAdTitle.FromString("Title"));
            _sut.UpdatePrice(Price.FromDecimal(100.10m, "USD", _mock.Object));

            //  Act
            Action action = () => _sut.RequestToPublish();

            //  Assert
            action.Should().Throw<InvalidEntityStateException>();
        }

        [TestMethod]
        public void Cannot_publish_without_price()
        {
            //  Arrange
            _sut.SetTitle(ClassifiedAdTitle.FromString("Title"));
            _sut.UpdateText(ClassifiedAdText.FromString("Text"));

            //  Act
            Action action = () => _sut.RequestToPublish();

            //  Assert
            action.Should().Throw<InvalidEntityStateException>();
        }

        [TestMethod]
        public void Cannot_publish_with_zero_price()
        {
            //  Arrange
            _sut.SetTitle(ClassifiedAdTitle.FromString("Title"));
            _sut.UpdateText(ClassifiedAdText.FromString("Text"));
            _sut.UpdatePrice(Price.FromDecimal(0m, "USD", _mock.Object));

            //  Act
            Action action = () => _sut.RequestToPublish();

            //  Assert
            action.Should().Throw<InvalidEntityStateException>();
        }
    }
}