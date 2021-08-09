using System;
using System.Linq;

namespace Teller.Logic
{
    public class TellerCurrency
    {
        public string CurrencyName { get; }
        public LegalTenderList LegalTenderList { get; }
        private int smallestLegalTender { get; }

        public TellerCurrency(string currencyName, LegalTenderList legalTenderList)
        {
            CurrencyName = currencyName;
            LegalTenderList = legalTenderList;
            smallestLegalTender = LegalTenderList.GetDefinitions().Min(definition => definition.Value);
        }

        public LegalTenderDefinition GetLegalTender(int value)
        {
            var definitions = LegalTenderList.GetDefinitions();

            try
            {
                return definitions.Single(definition => definition.Value == value);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidLegalTenderException("Invalid legal tender specified", ex);
            }

        }

        public int Rounded(int price)
        {
            var remainder = price % smallestLegalTender;
            if (remainder <= smallestLegalTender / 2)
            {
                return price - remainder;
            }
            return price - remainder + 5;
        }
    }
}
