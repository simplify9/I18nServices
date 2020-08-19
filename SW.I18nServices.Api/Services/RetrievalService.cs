using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService
{
    public class RetrievalService
    {
        private PlaceDb db;

        public RetrievalService(PlaceDb db)
        {
            this.db = db;
        }

        private async void ValidateFields(IDictionary<string, string> filters, IList<string> fields)
        {
            List<string> validFields = new List<string>
            {
                "Region1", "Locality", "Postcode",
                "Region2", "Region3", "Region4"
            };
            List<string> validFilters = new List<string> {"Locality", "Postcode", "Region1"};

            foreach (string field in fields)
                if (!validFields.Contains(field)) throw new Exception("Invalid Field");
            foreach(string filter in filters.Keys)
                if (!validFilters.Contains(filter)) throw new Exception("Invalid Filter");
        }
        public async Task<IEnumerable<IDictionary<string, string>>> GetPlaceData(IDictionary<string, string> filters, IList<string> fields, string countryCode, string lang = null)
        {
            ValidateFields(filters, fields);
            IEnumerable<IDictionary<string, string>> results = await db.RetrievePlaces(countryCode, fields, filters);
            return results;
        }

        public async Task<HashSet<string>> GetLocalities(string countryCode, string filter)
        {
            HashSet<string> results = await db.RetrieveLocalities(countryCode, filter);
            return results;
        }
    }
}
