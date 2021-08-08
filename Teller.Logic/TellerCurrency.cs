using System.Collections.Generic;

namespace Teller.Logic
{
    public class TellerCurrency
    {
        public string CurrencyName { get; }
        public LegalTenderList LegalTenderList { get; }

        public TellerCurrency(string currencyName, LegalTenderList legalTenderList)
        {
            CurrencyName = currencyName;
            LegalTenderList = legalTenderList;
        }
    }
}
