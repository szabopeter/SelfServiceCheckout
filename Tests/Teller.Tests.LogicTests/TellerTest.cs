using System;
using Xunit;
using Teller.Logic;

namespace Teller.Tests.LogicTests
{
    public class TellerTest
    {
        [Fact]
        public void Initialization()
        {
            var sut = new Teller.Logic.Teller(new TellerCurrencyFactory().GetHuf());
        }
    }
}
