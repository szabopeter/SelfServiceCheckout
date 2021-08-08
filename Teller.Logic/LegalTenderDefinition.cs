using System;

namespace Teller.Logic
{
    /// <summary>
    /// Definition of a note/coin, eg. 2 EUR coin
    /// </summary>
    public class LegalTenderDefinition
    {
        /// <summary>
        /// The value of the note/coin in the smallest unit of the currency 
        /// eg. 100 for 1 EUR, 100 for 100 HUF
        /// </summary>
        public int Value { get; }
        
        /// <summary>
        /// Max number of notes/coins that the machine can hodl
        /// </summary>
        public int MaxCount { get; }

        public LegalTenderDefinition(int value, int maxCount)
        {
            Value = value;
            MaxCount = maxCount;
        }

        internal static LegalTenderDefinition FromValue(int value, int maxCount) => new LegalTenderDefinition(value, maxCount);

        public LegalTenderStock MakeStock(int count) => new LegalTenderStock(this, count);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is LegalTenderDefinition other))
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}