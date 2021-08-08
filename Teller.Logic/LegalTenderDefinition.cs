using System;

namespace Teller.Logic
{
    public class LegalTenderDefinition
    {
        public int Value { get; }

        public LegalTenderDefinition(int value)
        {
            Value = value;
        }

        internal static LegalTenderDefinition FromValue(int value) => new LegalTenderDefinition(value);
    }
}