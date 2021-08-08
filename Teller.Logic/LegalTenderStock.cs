namespace Teller.Logic
{
    public class LegalTenderStock
    {
        public int Value { get; }
        public int Count { get; }

        public LegalTenderStock(int value, int count)
        {
            Value = value;
            Count = count;
        }
    }
}