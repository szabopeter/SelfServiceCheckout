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
            // TODO Allow specifying maxCount
            return new LegalTenderList(values.Select(value => LegalTenderDefinition.FromValue(value, 9999)));
        }

        public TellerStock InitializeStock()
        {
            var stocks = definitions.Select(definition => new LegalTenderStock(definition, 0));
            return new TellerStock(stocks);
        }
    }
}