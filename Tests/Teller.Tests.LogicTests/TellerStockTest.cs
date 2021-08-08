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

        [Theory]
        [InlineData(100, 2, 2, 3)]
        [InlineData(20, 0, 4, 1)]
        public void StockUpNonEmpty(int legalTender, int initial, int repeats, int toAdd)
        {
            // Arrange
            var hundredNote = defaultCurrency.GetLegalTender(legalTender);
            var stock = new TellerStock(hundredNote.MakeStock(initial));

            // Act
            for (var i = 0; i < repeats; i++)
            {
                stock = stock.StockUp(hundredNote.MakeStock(toAdd));
            }

            // Assert
            Assert.Equal(initial + repeats * toAdd, stock.GetCount(hundredNote));
        }

        [Fact]
        public void StockUpEmpty()
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
