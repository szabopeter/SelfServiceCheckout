using System;
using Xunit;

namespace Teller.Tests.LogicTests
{
    public class TellerTest
    {
        [Fact]
        public void Initialization()
        {
            var sut = new Teller.Logic.Teller();
        }
    }
}
