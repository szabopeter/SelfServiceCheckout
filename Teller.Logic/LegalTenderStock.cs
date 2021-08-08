using System;

namespace Teller.Logic
{
    public class LegalTenderStock
    {
        public LegalTenderDefinition LegalTender { get; }
        public int Count { get; }

        public LegalTenderStock(LegalTenderDefinition legalTenderDefinition, int count)
        {
            LegalTender = legalTenderDefinition ?? throw new ArgumentNullException(nameof(legalTenderDefinition));
            Count = count;
            if (Count > legalTenderDefinition.MaxCount)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is LegalTenderStock other))
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            return Count == other.Count && LegalTender.Equals(other.LegalTender);
        }

        public override int GetHashCode()
        {
            return Count.GetHashCode();
        }
    }
}