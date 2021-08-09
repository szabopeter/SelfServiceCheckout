using System.Collections.Generic;
using NLog;

namespace Teller.Logic
{
    public class Teller
    {
        public TellerCurrency PrimaryCurrency { get; }
        private readonly TellerCurrencyCollection additionalCurrencies;
        private readonly Dictionary<TellerCurrency, TellerStock> currencyStock;
        private readonly object currencyStockLock = new object();
        private readonly ILogger logger;

        public Teller(TellerCurrency primaryCurrency)
        {
            logger = LogManager.GetCurrentClassLogger();
            this.PrimaryCurrency = primaryCurrency;
            additionalCurrencies = new TellerCurrencyCollection();
            currencyStock = new Dictionary<TellerCurrency, TellerStock>{
                {primaryCurrency, primaryCurrency.LegalTenderList.InitializeStock()}
            };
        }

        public TellerStock GetStock()
        {
            lock (currencyStockLock)
            {
                return currencyStock[PrimaryCurrency];
            }
        }

        /// <summary>
        /// For managing the stocks, add/remove legal tenders by personnel
        /// </summary>
        /// <param name="stockUp">The delta change, positive values for adding, negatives for removing</param>
        public void Stock(TellerStock stockUp)
        {
            lock (currencyStockLock)
            {
                currencyStock[PrimaryCurrency] = currencyStock[PrimaryCurrency].StockUp(stockUp);
            }
        }

        public TellerStock Checkout(TellerStock inserted, int price)
        {
            price = PrimaryCurrency.Rounded(price);
            logger.Info("Checking out for a price of {price}", price);
            logger.Debug("Stock was around {stock}", currencyStock[PrimaryCurrency].ToString());
            logger.Debug("Inserted is {inserted}", inserted.ToString());
            var giveBackValue = inserted.TotalValue() - price;
            if (giveBackValue < 0)
            {
                throw new InsufficientInsertionException();
            }

            lock (currencyStockLock)
            {
                currencyStock[PrimaryCurrency] = currencyStock[PrimaryCurrency].StockUp(inserted);
                var (newStock, giveBack) = currencyStock[PrimaryCurrency].PayOut(giveBackValue);
                currencyStock[PrimaryCurrency] = newStock;
                // can save state here
                return giveBack;
            }
        }
    }
}
