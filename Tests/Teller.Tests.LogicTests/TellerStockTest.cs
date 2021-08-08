using Xunit;
using Teller.Logic;

namespace Teller.Tests.LogicTests
{
    public class TellerStockTest
    {
        private readonly TellerCurrency defaultCurrency;

        public TellerStockTest()
        {
            defaultCurrency = new TellerCurrencyFactory().GetHuf();
        }

        [Fact]
        public void ValueCalculation()
        {
            const int expectedValue = 3500;
            const int coin1 = 1000;
            const int count1 = 3;
            const int coin2 = 500;
            const int count2 = 1;

            // Arrange
            var inserted = new TellerStock(new[] {
                defaultCurrency.GetLegalTender(coin1).MakeStock(count1),
                defaultCurrency.GetLegalTender(coin2).MakeStock(count2),
            });

            // Act
            var actualValue = inserted.TotalValue();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void StockUp()
        {
            // Arrange
            var stock = new TellerStock(new LegalTenderStock[0]);
            
            // Act
            stock = stock.StockUp(defaultCurrency.GetLegalTender(1000).MakeStock(2));

            // Assert
            Assert.Equal(2000, stock.TotalValue());
        }
    }
}
