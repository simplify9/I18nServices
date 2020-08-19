using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18nServices.Api.Domain
{
    public class Place
    {
        public string CountryCode { get; set; }
        public string Language { get; set; }
		public string Region1 { get; set; }
		public string Region2 { get; set; }
		public string Region3 { get; set; }
		public string Region4 { get; set; }
		public string Locality { get; set; }
		public string Postcode { get; set; }
		public string Suburb { get; set; }
		public decimal Longitude { get; set; }
		public decimal Latitude { get; set; }

        public class Map : ClassMap<Place>
        {
            public Map()
            {
                Map(m => m.CountryCode).Name("iso");//.TypeConverter<ConvertStringToLong>();
                Map(m => m.Language).Name("language");
                Map(m => m.Region1).Name("region1");
                Map(m => m.Region2).Name("region2");
                Map(m => m.Region3).Name("region3");
                Map(m => m.Region4).Name("region4");
                Map(m => m.Locality).Name("locality");
                Map(m => m.Postcode).Name("postcode");
                Map(m => m.Suburb).Name("suburb");
                Map(m => m.Latitude).Name("latitude");
                Map(m => m.Longitude).Name("longitude");
            }
        }

    }
}
