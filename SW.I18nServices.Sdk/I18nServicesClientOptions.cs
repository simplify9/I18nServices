using Microsoft.Extensions.Configuration;
using SW.HttpExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18nServices.Sdk
{
    public class I18nServicesClientOptions : ApiClientOptionsBase
    {
        public override string ConfigurationSection => "I18nServicesClient";
    }
}
