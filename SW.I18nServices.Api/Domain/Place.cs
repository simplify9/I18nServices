using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18nServices.Api.Domain
{
    public class Place
    {
		public int Id { get; set; }
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
	}
}
