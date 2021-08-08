using System;
using System.Collections.Generic;

namespace Teller.Logic
{
    public class Teller
    {
        private readonly TellerCurrency primaryCurrency;
        private readonly TellerCurrencyCollection additionalCurrencies;
        private readonly Dictionary<TellerCurrency, TellerStock> currencyStock;

        public Teller(TellerCurrency primaryCurrency)
        {
            this.primaryCurrency = primaryCurrency;
            additionalCurrencies = new TellerCurrencyCollection();
            currencyStock = new Dictionary<TellerCurrency, TellerStock>{
                {primaryCurrency, primaryCurrency.LegalTenderList.InitializeStock()}
            };
        }

        public TellerStock GetStock()
        {
            return currencyStock[primaryCurrency];
        }

        /// <summary>
        /// For managing the stocks, add/remove legal tenders by personnel
        /// </summary>
        /// <param name="stockUp">The delta change, positive values for adding, negatives for removing</param>
        public void Stock(TellerStock stockUp)
        {
            currencyStock[primaryCurrency] = currencyStock[primaryCurrency].StockUp(stockUp);
        }

        public TellerStock Checkout(TellerStock inserted, int price)
        {
            return null;
        }
    }
}
