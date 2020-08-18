

using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18n
{
    public class PhoneNumberingPlansService
    {

        private readonly IDictionary<string, PnpValue> pnpd;
        private readonly CountriesService countriesService;

        public PhoneNumberingPlansService(CountriesService countriesService)
        {
            var assembly = typeof(I18nService).Assembly;
            var pnpds = assembly.GetManifestResourceStream("SW.I18n.Data.pnpd.bin");
            pnpd = pnpds.AsDictionary<PnpValue>();
            this.countriesService = countriesService;
        }

        public PnpResult Validate(string phone, string countryCode = null)
        {
            var ret = new PnpResult()
            {
                Status = PnpResultStatus.BadPhone
            };

            if (string.IsNullOrWhiteSpace(phone)) return ret;

            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                if (countriesService.TryGet(countryCode.ToUpper(), out var cnrty))
                {
                    if (!phone.StartsWith(cnrty.Phone))
                    {
                        phone = string.Concat(cnrty.Phone, phone);
                    }
                }
            }

            string num = string.Empty;

            foreach (var c in phone) if (char.IsDigit(c)) num = string.Concat(num, c);

            PnpValue pnp = null;

            for (int i = num.Length; i > 3; i--)
            {

                pnpd.TryGetValue(num.Substring(0, i), out pnp);
                if (pnp != null) break;

            }

            if (pnp == null) return ret;
            if (pnp.MinLength == null || pnp.MaxLength == null) return ret;

            var cntrynumlen = countriesService.Get(pnp.Country).Phone.ToString().Length;
            var localnumlen = num.Length - cntrynumlen - pnp.AreaCodeLength;

            if (localnumlen < (int)pnp.MinLength)
            {
                ret.Status = PnpResultStatus.TooShort;
                return ret;
            }

            if (localnumlen > (int)pnp.MaxLength)
            {
                ret.Status = PnpResultStatus.TooLong;
                return ret;
            }


            switch (pnp.Type)
            {
                case "MOB":
                    ret.PhoneType = PhoneType.Mobile;
                    break;
                case "FIX":
                    ret.PhoneType = PhoneType.Landline;
                    break;
                default:
                    ret.PhoneType = PhoneType.Other;
                    break;

            }

            ret.CountryCode = pnp.Country;
            ret.PhoneNumber = long.Parse(num);
            ret.PhoneNumberShort = long.Parse(num.Substring(cntrynumlen));
            ret.Status = PnpResultStatus.Ok;

            return ret;
        }

    }
}
