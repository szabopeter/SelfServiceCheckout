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

        public int GetCount(LegalTenderDefinition legalTender)
            => Stocks.Single(stock => stock.LegalTender == legalTender).Count;

        public void CheckForNegative()
        {
            if (Stocks.Any(stock => stock.Count < 0))
            {
                // throw new NegativeStockException();
                throw new InvalidOperationException();
            }
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

        /// <summary>
        /// Calculate the change to be given back and the remaining stock
        /// </summary>
        /// <param name="giveBackValue"></param>
        /// <returns>New stock and the stock to give back</returns>
        public (TellerStock, TellerStock) PayOut(int giveBackValue)
        {
            Console.WriteLine($"Giving back {giveBackValue}");
            var stockLeft = this;
            var giveBack = new TellerStock(new LegalTenderStock[0]);
            var legalTendersAvailable = Stocks
                .Select(stock => stock.LegalTender)
                .OrderByDescending(legalTender => legalTender.Value)
                .ToArray();

            foreach (var legalTender in legalTendersAvailable)
            {
                while (legalTender.Value <= giveBackValue)
                {
                    if (stockLeft.GetCount(legalTender) < 1)
                    {
                        break;
                    }

                    stockLeft = stockLeft.StockUp(new LegalTenderStock(legalTender, -1));
                    giveBack = giveBack.StockUp(new LegalTenderStock(legalTender, 1));
                    giveBackValue -= legalTender.Value;
                }
            }

            return (stockLeft, giveBack);
        }

        public override int GetHashCode()
        {
            return Stocks.Count();
        }
    }
}