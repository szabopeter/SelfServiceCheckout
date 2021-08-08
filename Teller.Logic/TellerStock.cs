using System.Collections.Generic;
using System.Linq;

namespace Teller.Logic
{
    public class TellerStock
    {
        public IEnumerable<LegalTenderStock> Stocks { get; }

        public TellerStock(IEnumerable<LegalTenderStock> stocks)
        {
            Stocks = stocks.ToList();
        }
    }
}