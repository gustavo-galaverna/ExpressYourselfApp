namespace ExpressYourself.Application.Models.IpAddresses;

public record IpDetailResponse
{
    public string? IpAddress { get; set; }
    public string? CountryName { get; set; }
    public string? TwoLetterCode { get; set; }
    public string? ThreeLetterCode { get; set; }

}