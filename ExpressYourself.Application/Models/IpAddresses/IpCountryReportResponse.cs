namespace ExpressYourself.Application.Models.IpAddresses;
    public record IpCountryReportResponse
    {
        public string? CountryName { get; set; }
        public int AddressesCount { get; set; }
        public DateTime LastAddressUpdated { get; set; }
    }

