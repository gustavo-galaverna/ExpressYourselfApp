namespace ExpressYourself.Domain.Entities;

public class IpCountryReport
{
    public string? CountryName { get; set; }
    public int AddressesCount { get; set; }
    public DateTime LastAddressUpdated { get; set; }
}

