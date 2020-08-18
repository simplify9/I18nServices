using System;
using System.Collections.Generic;
using System.Text;

namespace SW.I18n
{

    public class Pnp : PnpValue
    {
        public long CNS { get; set; }

    }


    public class PnpValue
    {
        public PnpValue() { }


        public PnpValue(string Input)
        {
            var value = Input.Split(';');
            Country = value[0];
            Type = value[1];
            if (short.TryParse(value[2], out var minsubval)) MinLength = minsubval;
            if (short.TryParse(value[3], out var maxsubval)) MaxLength = maxsubval;
            if (short.TryParse(value[4], out var acl)) AreaCodeLength = acl;

        }

        public string Country { get; set; }
        //public Country Country { get; set; }
        public string Type { get; set; }
        //public string Location { get; set; }
        public short? AreaCodeLength { get; set; }
        public short? MinLength { get; set; }
        //public short InternationalCode { get; set; }
        public short? MaxLength { get; set; }
        //public string Registrar { get; set; }
        //public string CreatedBy { get; set; }

        public override string ToString()
        {
            return $"{Country};{Type};{MinLength};{MaxLength};{AreaCodeLength}";
        }

    }


}
