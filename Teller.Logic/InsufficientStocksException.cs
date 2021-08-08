namespace Teller.Logic
{
    [System.Serializable]
    public class InsufficientStocksException : System.Exception
    {
        public InsufficientStocksException() { }
        public InsufficientStocksException(string message) : base(message) { }
        public InsufficientStocksException(string message, System.Exception inner) : base(message, inner) { }
        protected InsufficientStocksException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
