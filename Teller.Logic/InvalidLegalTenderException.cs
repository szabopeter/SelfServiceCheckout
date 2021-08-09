namespace Teller.Logic
{
    [System.Serializable]
    public class InvalidLegalTenderException : TellerException
    {
        public InvalidLegalTenderException() { }
        public InvalidLegalTenderException(string message) : base(message) { }
        public InvalidLegalTenderException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidLegalTenderException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
