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
            return LegalTenderList.GetDefinitions().Single(definition => definition.Value == value);
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
