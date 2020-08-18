using System;
using System.Collections.Generic;
using System.Text;

namespace SW.PostCodes.Api.Domain
{
    class CountrySetting
    {
        public string Code { get; set; }
        public string PostcodeName { get; set; }
        public bool PostcodeRequired { get; set; }
        public string PostcodeFormat { get; set; }
        public string PostcodeRegex { get; set; }
        public ICollection<string> Languages { get; set; }
        public string Name { get; set; }
        public bool Region1Required { get; set; }
        public string Region1Name { get; set; }
        public bool Region2Required { get; set; }
        public string Region2Name { get; set; }
        public bool Region3Required { get; set; }
        public string Region3Name { get; set; }
        public bool Region4Required { get; set; }
        public string Region4Name { get; set; }
        public bool LocalityRequired { get; set; }
        public string LocalityName { get; set; }

    }
}
