using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Teller.Logic;

namespace Teller.Tests.LogicTests
{
    public class TellerTest
    {
        private readonly TellerCurrency defaultCurrency;
        private readonly Teller.Logic.Teller sut;

        public TellerTest()
        {
            defaultCurrency = new TellerCurrencyFactory().GetHuf();
            sut = new Teller.Logic.Teller(defaultCurrency);
        }

        [Fact]
        public void Initialization()
        {
            // Assert
            var actualStock = sut.GetStock();
            Assert.All(actualStock.Stocks.Select(stock => stock.Count), count => Assert.Equal(0, count));
        }

        [Fact]
        public void StockUp()
        {
            // Arrange
            var expectedStock = defaultCurrency.LegalTenderList.InitializeStock();
            var expectedCount = 1;
            foreach (var legalTender in defaultCurrency.LegalTenderList.GetDefinitions())
            {
                expectedStock = expectedStock.StockUp(new LegalTenderStock(legalTender, expectedCount));
                expectedCount *= 2;
            }

            // Act
            sut.Stock(expectedStock);

            // Assert
            var actualStock = sut.GetStock();
            Assert.Equal(expectedStock, actualStock);
        }

        [Fact]
        public void Checkout()
        {
            // Arrange
            StockUpToMax();
            var price = 3200;
            var inserted = new TellerStock(new[] {
                defaultCurrency.GetLegalTender(1000).MakeStock(3),
                defaultCurrency.GetLegalTender(500).MakeStock(1),
            });

            // Act
            var givenBack = sut.Checkout(inserted, price);

            // Assert
            var expectedTotal = price;
            var actualTotal = inserted.StockUp(givenBack).TotalValue();
            Assert.Equal(expectedTotal, actualTotal);
        }

        private void StockUpToMax()
        {
            sut.Stock(new TellerStock(defaultCurrency.LegalTenderList.GetDefinitions()
                .Select(legalTender => legalTender.MakeStock(legalTender.MaxCount))));
        }
    }
}
