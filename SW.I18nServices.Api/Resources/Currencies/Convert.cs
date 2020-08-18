using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18n.Resources.Currencies
{
    [HandlerName("convert")]
    class Convert : IQueryHandler<ConvertCurrency>
    {
        private readonly CurrenciesService currenciesService;
        private readonly I18nOptions i18NOptions;

        public Convert(CurrenciesService currenciesService, I18nOptions i18NOptions)
        {
            this.currenciesService = currenciesService;
            this.i18NOptions = i18NOptions;
        }

        async public Task<object> Handle(ConvertCurrency request)
        {
            if (request.Value == 0)
                throw new SWException("Zero value found in request.");
            if (request.From == null && request.To == null)
                throw new SWException("Missing 'from' and/or 'to' in request.");
            else if (request.From == null)
                return await currenciesService.ConvertAsync(request.Value, i18NOptions.BaseCurrency, request.To);
            else if (request.To == null)
                return await currenciesService.ConvertAsync(request.Value, request.From, i18NOptions.BaseCurrency);
            else
                return await currenciesService.ConvertAsync(request.Value, request.From, request.To);

        }
    }
}
