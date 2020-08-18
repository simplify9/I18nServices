using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW.I18n
{
    public class CurrenciesService
    {
        private readonly IDictionary<string, Currency> crncyd;
        private readonly IMemoryCache memoryCache;
        private readonly ExternalCurrencyRatesService externalCurrencyRatesService;
        private readonly I18nOptions i18nOptions;

        //private const string currencyRateApiURL = "https://data.fixer.io/api/latest?access_key=44f98ba3914137a6248c67a8d87aea79&base=usd";

        public CurrenciesService(CountriesService countriesService, IMemoryCache memoryCache, ExternalCurrencyRatesService externalCurrencyRatesService, I18nOptions i18nOptions)
        {
            crncyd = new Dictionary<string, Currency>(StringComparer.OrdinalIgnoreCase);

            foreach (var c in countriesService.List())

                if (!string.IsNullOrWhiteSpace(c.CurrencyCode) && !crncyd.ContainsKey(c.CurrencyCode))

                    crncyd.Add(c.CurrencyCode, new Currency
                    {
                        Code = c.CurrencyCode,
                        Name = c.CurrencyName
                    });

            this.memoryCache = memoryCache;
            this.externalCurrencyRatesService = externalCurrencyRatesService;
            this.i18nOptions = i18nOptions;
        }

        public IEnumerable<Currency> List()
        {
            return crncyd.Values;
        }

        public Currency Get(string currency)
        {
            crncyd.TryGetValue(currency, out var c);
            return c;
        }

        public bool TryGet(string currency, out Currency country)
        {
            if (crncyd.TryGetValue(currency, out country)) return true;
            return false;
        }

        async public Task<decimal> ConvertAsync(decimal value, string fromCurrency, string toCurrency)
        {
            if (fromCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
                return value;

            if (value == 0)
                return value;

            var rates = await FetchRatesAsync();

            rates.Rates.TryGetValue(fromCurrency, out decimal fromXrate);
            rates.Rates.TryGetValue(toCurrency, out decimal toXrate);

            if (fromXrate > 0)
                return (toXrate / fromXrate) * value;
            else
                return 0;
        }

        async private Task<CurrencyRates> FetchRatesAsync()
        {
            if (memoryCache.TryGetValue("currency_rates", out CurrencyRates rates))
                return rates;

            rates = await externalCurrencyRatesService.GetRates();
            return memoryCache.Set("currency_rates", rates, TimeSpan.FromMinutes(i18nOptions.CurrencyRatesCacheDuration));
        }
    }
}
