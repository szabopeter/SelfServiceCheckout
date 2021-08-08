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
            var sut = new Teller.Logic.Teller(new TellerCurrency("HUF", LegalTenderList.FromValues(5, 10, 20, 100, 200, 500, 1000, 5000, 10000, 20000)));
        }
    }
}
