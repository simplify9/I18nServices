using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService.Api.Resources.Countries
{
    class Get : IGetHandler<string>
    {
        private readonly I18nServiceService i18NService;

        public Get(I18nServiceService i18NService)
        {
            this.i18NService = i18NService;
        }

        async public Task<object> Handle(string key, bool lookup = false)
        {
            var country = i18NService.Countries.Get(key);

            if (lookup)
            {
                return country?.Name;
            }

            return country;
        }
    }
}
