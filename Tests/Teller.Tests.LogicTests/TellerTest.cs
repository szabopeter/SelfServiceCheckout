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

        [Theory]
        [InlineData(3000)]
        [InlineData(3200)]
        [InlineData(3500)] 
        [InlineData(3005)]
        public void Checkout(int price)
        {
            // Arrange
            StockUpPlenty();
            var inserted = new TellerStock(new[] {
                defaultCurrency.GetLegalTender(1000).MakeStock(3),
                defaultCurrency.GetLegalTender(500).MakeStock(1),
            });

            // Precondition
            const int insertedSum = 3500;
            Assert.Equal(insertedSum, inserted.TotalValue());

            // Act
            var givenBack = sut.Checkout(inserted, price);

            // Assert
            var expectedGiveBackSum = insertedSum - price;
            Assert.Equal(expectedGiveBackSum, givenBack.TotalValue());
        }

        [Fact]
        public void CheckoutInsufficient()
        {
            StockUpPlenty();
            // Act & Assert
            Assert.ThrowsAny<InsufficientInsertionException>(
                () => sut.Checkout(new TellerStock(new LegalTenderStock[0]), 1000)
            );
        }

        [Fact]
        public void StockInsufficient()
        {
            // Act & Assert
            Assert.ThrowsAny<InsufficientStocksException>(
                () => sut.Checkout(new TellerStock(defaultCurrency.GetLegalTender(100).MakeStock(1)), 80)
            );
        }

        [Fact]
        public void StockSufficientFromSmallerUnits()
        {
            // Arrange
            var tenners = defaultCurrency.GetLegalTender(10);
            var fivers = defaultCurrency.GetLegalTender(5);
            sut.Stock(new TellerStock(new[] {
                tenners.MakeStock(3),
                fivers.MakeStock(4),
            }));

            // Act
            var giveBack = sut.Checkout(new TellerStock(defaultCurrency.GetLegalTender(100).MakeStock(1)), 50);

            // Assert
            Assert.Equal(3, giveBack.GetCount(tenners));
            Assert.Equal(4, giveBack.GetCount(fivers));
        }

        public enum Sufficiency { Sufficient, Insufficient }

        [Theory]
        [InlineData(19, 4, Sufficiency.Sufficient)]
        [InlineData(20, 4, Sufficiency.Sufficient)]
        [InlineData(21, 4, Sufficiency.Sufficient)]
        [InlineData(22, 4, Sufficiency.Sufficient)]
        [InlineData(23, 4, Sufficiency.Insufficient)]
        [InlineData(23, 5, Sufficiency.Sufficient)]
        public void Rounding(int price, int fivers, Sufficiency expectedSufficiency)
        {
            // Arrange
            StockUpPlenty();
            var insert = new TellerStock(defaultCurrency.GetLegalTender(5).MakeStock(fivers));

            // Act
            var actualSufficiency = Sufficiency.Sufficient;
            try
            {
                var giveBack = sut.Checkout(insert, price);
                Assert.Equal(0, giveBack.TotalValue());
            } catch (InsufficientInsertionException)
            {
                actualSufficiency = Sufficiency.Insufficient;
            }

            Assert.Equal(expectedSufficiency, actualSufficiency);
        }

        private void StockUpPlenty()
        {
            sut.Stock(new TellerStock(defaultCurrency.LegalTenderList.GetDefinitions()
                .Select(legalTender => legalTender.MakeStock(legalTender.MaxCount / 2))));
        }
    }
}
