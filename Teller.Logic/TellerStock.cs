using System;
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

        internal TellerStock StockUp(TellerStock stockUp)
        {
            return new TellerStock(Stocks.Union(stockUp.Stocks)
                .GroupBy(
                    legalTenderStock => legalTenderStock.Value, 
                    legalTenderStock => legalTenderStock.Count,
                    (value, items) => new LegalTenderStock(value, items.Sum())
                )
            );
        }
    }
}