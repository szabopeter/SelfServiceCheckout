using System.Collections.Generic;
using NLog;

namespace Teller.Logic
{
    public class Teller
    {
        private readonly TellerCurrency primaryCurrency;
        private readonly TellerCurrencyCollection additionalCurrencies;
        private readonly Dictionary<TellerCurrency, TellerStock> currencyStock;
        private readonly object currencyStockLock = new object();
        private readonly ILogger logger;

        public Teller(TellerCurrency primaryCurrency)
        {
            logger = LogManager.GetCurrentClassLogger();
            this.primaryCurrency = primaryCurrency;
            additionalCurrencies = new TellerCurrencyCollection();
            currencyStock = new Dictionary<TellerCurrency, TellerStock>{
                {primaryCurrency, primaryCurrency.LegalTenderList.InitializeStock()}
            };
        }

        public TellerStock GetStock()
        {
            lock (currencyStockLock)
            {
                return currencyStock[primaryCurrency];
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
                currencyStock[primaryCurrency] = currencyStock[primaryCurrency].StockUp(stockUp);
            }
        }

        public TellerStock Checkout(TellerStock inserted, int price)
        {
            price = primaryCurrency.Rounded(price);
            logger.Info("Checking out for a price of {price}", price);
            logger.Debug("Stock was around {stock}", currencyStock[primaryCurrency].ToString());
            logger.Debug("Inserted is {inserted}", inserted.ToString());
            var giveBackValue = inserted.TotalValue() - price;
            if (giveBackValue < 0)
            {
                throw new InsufficientInsertionException();
            }

            lock (currencyStockLock)
            {
                currencyStock[primaryCurrency] = currencyStock[primaryCurrency].StockUp(inserted);
                var (newStock, giveBack) = currencyStock[primaryCurrency].PayOut(giveBackValue);
                currencyStock[primaryCurrency] = newStock;
                // can save state here
                return giveBack;
            }
        }
    }
}
