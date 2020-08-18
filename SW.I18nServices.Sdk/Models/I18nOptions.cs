using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18n
{
    public class I18nOptions
    {
        public const string ConfigurationSection = "I18n";
        public string CurrencyRatesUrl { get; set; }
        public int CurrencyRatesCacheDuration { get; set; } = 60;
        public string BaseCurrency { get; set; } = "usd";
    }
}
