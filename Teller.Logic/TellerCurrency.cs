using System.Collections.Generic;
using System.Linq;

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

        public LegalTenderDefinition GetLegalTender(int value)
        {
            return LegalTenderList.GetDefinitions().Single(definition => definition.Value == value);
        }
    }
}
