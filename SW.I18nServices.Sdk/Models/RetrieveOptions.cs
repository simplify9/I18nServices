using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18nService.Model
{
    public class RetrieveOptions
    {
        public string Locality { get; set; }
        public string Postcode { get; set; }
        public string Region1 { get; set; }
        public string Fields { get; set; }
        public string Lang { get; set; }
    }
}
