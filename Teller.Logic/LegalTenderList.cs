using System.Collections.Generic;
using System.Linq;

namespace Teller.Logic
{
    public class LegalTenderList
    {
        private readonly List<LegalTenderDefinition> definitions;

        public LegalTenderList(IEnumerable<LegalTenderDefinition> definitions)
        {
            this.definitions = definitions.ToList();
        }

        public static LegalTenderList FromValues(params int[] values)
        {
            return new LegalTenderList(values.Select(LegalTenderDefinition.FromValue));
        }
    }
}