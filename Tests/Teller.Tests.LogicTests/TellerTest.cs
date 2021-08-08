using System;
using System.Linq;
using Xunit;
using Teller.Logic;

namespace Teller.Tests.LogicTests
{
    public class TellerTest
    {
        [Fact]
        public void Initialization()
        {
            var huf = new TellerCurrencyFactory().GetHuf();
            var sut = new Teller.Logic.Teller(huf);
            var actualStock = sut.GetStock();
            Assert.All(actualStock.Stocks.Select(stock => stock.Count), count => Assert.Equal(0, count));
        }
    }
}
