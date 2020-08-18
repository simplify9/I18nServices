using SW.I18nService.Model;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18n.Resources.Places
{
    public class Retrieve : IQueryHandler<string,RetrieveOptions>
    {
        readonly RetrievalService retrievalService;
        public Retrieve(RetrievalService retrievalService)
        {
            this.retrievalService = retrievalService;
        }

        public async Task<object> Handle(string countryCode, RetrieveOptions request)
        {
            IList<string> fields = request.Fields != null ? request.Fields.Split(',') : new string[] { };
            IDictionary<string, string> filters = new Dictionary<string, string>();

            if (request.Locality != null) filters["Locality"] = request.Locality;
            if (request.Postcode != null) filters["Postcode"] = request.Postcode;
            if (request.Region1 != null) filters["Region1"] = request.Region1;

            IEnumerable<IDictionary<string, string>> retrieved = await retrievalService.GetPlaceData(filters, fields, countryCode.ToUpper());
            return retrieved;
        }
    }
}
