using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SW.PrimitiveTypes;
using SW.HttpExtensions;
using System.Security.Claims;

namespace SW.I18nServices.Sdk
{
    public class I18nServicesClientMock : II18nServicesClient
    {
        private readonly I18nServicesClientOptions i18NServicesClientOptions;

        public I18nServicesClientMock(I18nServicesClientOptions i18NServicesClientOptions)
        {
            this.i18NServicesClientOptions = i18NServicesClientOptions;
        }

    }
}
