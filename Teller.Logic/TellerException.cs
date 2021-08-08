using System;
using System.Runtime.Serialization;

namespace Teller.Logic
{
    [Serializable]
    public class TellerException : Exception
    {
        public TellerException() { }
        public TellerException(string message) : base(message) { }
        public TellerException(string message, Exception inner) : base(message, inner) { }
        protected TellerException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}
