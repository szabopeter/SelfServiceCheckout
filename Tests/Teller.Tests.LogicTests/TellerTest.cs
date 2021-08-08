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

        private void StockUpPlenty()
        {
            sut.Stock(new TellerStock(defaultCurrency.LegalTenderList.GetDefinitions()
                .Select(legalTender => legalTender.MakeStock(legalTender.MaxCount / 2))));
        }
    }
}
