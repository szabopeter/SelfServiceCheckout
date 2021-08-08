namespace Teller.Logic
{
    public class TellerCurrencyFactory
    {
        public TellerCurrency GetHuf() => new TellerCurrency(
            "HUF", 
            LegalTenderList.FromValues(5, 10, 20, 100, 200, 500, 1000, 5000, 10000, 20000)
        );
    }
}
