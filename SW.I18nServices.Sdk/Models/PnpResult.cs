namespace SW.I18nService
{
    public class PnpResult
    {
        public long PhoneNumber { get; set; }
        public long PhoneNumberShort { get; set; }
        public string CountryCode { get; set; }
        public PhoneType PhoneType { get; set; }
        public PnpResultStatus Status { get; set; }
    }


}
