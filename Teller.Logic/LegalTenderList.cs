using System;
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
            // TODO this.definitions.ToDictionary(x=> x);
        }

        public IEnumerable<LegalTenderDefinition> GetDefinitions() => definitions;

        public static LegalTenderList FromValues(params int[] values)
        {
            return new LegalTenderList(values.Select(LegalTenderDefinition.FromValue));
        }

        internal IEnumerable<LegalTenderStock> InitializeStock()
        {
            return definitions.Select(definition => new LegalTenderStock(definition.Value, 0));
        }
    }
}