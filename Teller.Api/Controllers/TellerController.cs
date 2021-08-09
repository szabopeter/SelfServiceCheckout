using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Teller.Logic;

namespace Teller.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class TellerController : ControllerBase
    {
        private readonly ILogger<TellerController> _logger;
        private readonly Teller.Logic.Teller teller;

        public TellerController(ILogger<TellerController> logger, Teller.Logic.Teller teller)
        {
            _logger = logger;
            this.teller = teller;
        }

        [HttpGet("Stock")]
        public Dictionary<string, int> GetStock()
        {
            var stock = teller.GetStock();
            return GetDict(stock);
        }

        [HttpPost("Stock")]
        public Dictionary<string, int> PostStock(Dictionary<string, int> stockUp)
        {
            teller.Stock(GetStock(stockUp));
            return GetStock();
        }

        [HttpPost("Checkout")]
        public Dictionary<string, int> PostCheckout(CheckoutDto checkoutDto)
        {

            var inserted = GetStock(checkoutDto.Inserted);
            var giveBack = teller.Checkout(inserted, checkoutDto.Price);
            return GetDict(giveBack);
        }

        private TellerStock GetStock(Dictionary<string, int> stock)
        {
            return new TellerStock(stock.Select(
                valueAndCountPair => GetLegalTender(valueAndCountPair.Key).MakeStock(valueAndCountPair.Value)));
        }

        private Dictionary<string, int> GetDict(TellerStock stock)
        {
            return stock
                .Stocks
                .ToDictionary(
                    stock => stock.LegalTender.Value.ToString(),
                    stock => stock.Count
                );
        }

        private LegalTenderDefinition GetLegalTender(string key)
        {
            if (!int.TryParse(key, out var value))
            {
                throw new InvalidLegalTenderException();
            }

            return teller.PrimaryCurrency.GetLegalTender(value);
        }
    }
}
