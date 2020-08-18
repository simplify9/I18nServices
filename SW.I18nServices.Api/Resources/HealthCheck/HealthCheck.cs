using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nServices.Api.Resources.HealthCheck
{
    public class HealthCheck : IQueryHandler
    {
        public Task<object> Handle()
        {
            return Task.FromResult(new object());
        }
    }
}
