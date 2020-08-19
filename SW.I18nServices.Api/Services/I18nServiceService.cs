

using System.Collections.Generic;


using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace SW.I18nService
{
    public  class I18nServiceService
    {

        private readonly IServiceProvider serviceProvider;

        public CountriesService Countries => serviceProvider.GetService<CountriesService>();
        public CurrenciesService Currencies => serviceProvider.GetService<CurrenciesService>();
        public PhoneNumberingPlansService PhoneNumberingPlans => serviceProvider.GetService<PhoneNumberingPlansService>();

        public I18nServiceService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
