namespace Teller.Logic
{
    public class TellerCurrencyFactory
    {
        public TellerCurrency GetHuf() => new TellerCurrency(
            "HUF", 
            LegalTenderList.FromValues(5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000)
        );
    }
}
