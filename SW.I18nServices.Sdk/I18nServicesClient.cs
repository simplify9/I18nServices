using Microsoft.Extensions.Configuration;
using SW.HttpExtensions;
using SW.PrimitiveTypes;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SW.I18nServices.Sdk
{
    public class I18nServicesClient : ApiClientBase<I18nServicesClientOptions>, II18nServicesClient
    {
        public I18nServicesClient(HttpClient httpClient, RequestContext requestContext, I18nServicesClientOptions mtmClientOptions) : base(httpClient, requestContext, mtmClientOptions)
        {
        }


    }
}
