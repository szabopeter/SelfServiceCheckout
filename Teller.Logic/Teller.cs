using System;

namespace Teller.Logic
{
    public class Teller
    {
        private readonly TellerCurrency primaryCurrency;
        private readonly TellerCurrencyCollection additionalCurrencies;

        public Teller(TellerCurrency primaryCurrency)
        {
            this.primaryCurrency = primaryCurrency;
            this.additionalCurrencies = new TellerCurrencyCollection();
        }

        public TellerStock GetStock()
        {
            return new TellerStock(primaryCurrency.LegalTenderList.InitializeStock());
        }
    }
}
