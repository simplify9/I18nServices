using SW.EfCoreExtensions;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService.Api.Resources.Currencies
{
    class Search : ISearchyHandler
    {
        private readonly I18nServiceService i18NService;

        public Search(I18nServiceService i18NService)
        {
            this.i18NService = i18NService;
        }

        async public Task<object> Handle(SearchyRequest searchyRequest, bool lookup = false, string searchPhrase = null)
        {

            var qry = i18NService.Currencies.List()
                    .Where(c => string.IsNullOrWhiteSpace(searchPhrase) ? true : c.Code.ToLower().Contains(searchPhrase.ToLower())).AsQueryable(); 


            if (lookup)
            {
                IDictionary<string, string> dict = qry.OrderBy(c => c.Code)
                    .ToDictionary(c => c.Code, c => c.Code);

                return dict;
            }

            var result = qry
                .Search(searchyRequest.Conditions)
                .ToList();

            return Task.FromResult(new SearchyResponse<Currency>
            {
                Result = result,
                TotalCount = result.Count
            });
        }
    }
}
