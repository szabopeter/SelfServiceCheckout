using System.Collections.Generic;

namespace Teller.Api
{
    public class CheckoutDto
    {
        public Dictionary<string, int> Inserted { get; set; }
        public int Price { get; set; }
    }
}