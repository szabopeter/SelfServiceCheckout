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
            var expectedStock = new Dictionary<int, int>();
            var expectedCount = 1;
            foreach (var legalTender in defaultCurrency.LegalTenderList.GetDefinitions())
            {
                expectedStock[legalTender.Value] = expectedCount;
                expectedCount *= 2;
            }

            // Act
            var stockUp = new TellerStock(expectedStock.Select(valueCountPair => new LegalTenderStock(valueCountPair.Key, valueCountPair.Value)));
            sut.Stock(stockUp);

            // Assert
            var actualStock = sut.GetStock();
            Assert.Equal(expectedStock, actualStock.Stocks.ToDictionary(stock => stock.Value, stock => stock.Count));
        }
    }
}
