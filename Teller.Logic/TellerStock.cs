using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;

namespace Teller.Logic
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class TellerStock
    {
        public IEnumerable<LegalTenderStock> Stocks { get; }

        public TellerStock(IEnumerable<LegalTenderStock> stocks)
        {
            Stocks = stocks.ToList();
            LogManager.GetCurrentClassLogger().Debug("New TellerStock {stocks}", ToString());
        }

        public TellerStock() : this(new LegalTenderStock[0])
        {
        }

        public TellerStock(LegalTenderStock stock) : this(new[] { stock })
        {
        }

        public TellerStock StockUp(TellerStock stockUp)
        {
            var combined = Stocks.Concat(stockUp.Stocks);
            return new TellerStock(combined
                .GroupBy(
                    legalTenderStock => legalTenderStock.LegalTender,
                    legalTenderStock => legalTenderStock.Count,
                    (legalTender, counts) => legalTender.MakeStock(counts.Sum())
                )
            );
        }

        public TellerStock StockUp(LegalTenderStock stockUp) => StockUp(new TellerStock(stockUp));

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
            var stockLeft = this;
            var giveBack = new TellerStock();
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

                    stockLeft = stockLeft.StockUp(legalTender.MakeStock(-1));
                    giveBack = giveBack.StockUp(legalTender.MakeStock(1));
                    giveBackValue -= legalTender.Value;
                }
            }

            if (giveBackValue > 0)
            {
                throw new InsufficientStocksException();
            }

            return (stockLeft, giveBack);
        }

        public override int GetHashCode() => Stocks.Count();

        private string GetDebuggerDisplay() => ToString();

        public override string ToString() => FormatStocks(Stocks);

        private string FormatStocks(IEnumerable<LegalTenderStock> stocks)
        {
            return string.Join("+", stocks.Select(stock => $"{stock.LegalTender.Value}*{stock.Count}"));            
        }
   }
}