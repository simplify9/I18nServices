using SW.I18nService.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService.Resources.Localities
{
    public class GetCountryLocalities : IQueryHandler<string, LocalityOptions>
    {
        readonly RetrievalService retrievalService;
        public GetCountryLocalities(RetrievalService retrievalService)
        {
            this.retrievalService = retrievalService;
        }

        public async Task<object> Handle(string countryCode, LocalityOptions request)
        {
            var rs = await retrievalService.GetLocalities(countryCode, request.Locality);
            return rs;
        }
    }
}
