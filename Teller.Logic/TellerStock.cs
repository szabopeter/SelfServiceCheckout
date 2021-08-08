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

        public TellerStock StockUp(TellerStock stockUp)
        {
            return new TellerStock(Stocks.Union(stockUp.Stocks)
                .GroupBy(
                    legalTenderStock => legalTenderStock.LegalTender,
                    legalTenderStock => legalTenderStock.Count,
                    (legalTender, items) => new LegalTenderStock(legalTender, items.Sum())
                )
            );
        }

        public TellerStock StockUp(LegalTenderStock stockUp)
        {
            return StockUp(new TellerStock(new[] { stockUp }));
        }

        public int TotalValue()
        {
            return Stocks.Select(legalTenderStock => legalTenderStock.LegalTender.Value * legalTenderStock.Count).Sum();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is TellerStock other))
                return false;

            if (obj.GetType() != GetType())
            {
                return false;
            }

            var ours = Stocks.OrderBy(legalTenderStock => legalTenderStock.LegalTender.Value).ToList();
            var theirs = other.Stocks.OrderBy(legalTenderStock => legalTenderStock.LegalTender.Value).ToList();
            return ours.SequenceEqual(theirs);
        }

        public override int GetHashCode()
        {
            return Stocks.Count();
        }
    }
}