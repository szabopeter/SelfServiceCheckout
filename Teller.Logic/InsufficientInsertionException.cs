using System;
using System.Runtime.Serialization;

namespace Teller.Logic
{
    [System.Serializable]
    public class InsufficientInsertionException : TellerException
    {
        public InsufficientInsertionException() { }
        public InsufficientInsertionException(string message) : base(message) { }
        public InsufficientInsertionException(string message, Exception inner) : base(message, inner) { }
        protected InsufficientInsertionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
