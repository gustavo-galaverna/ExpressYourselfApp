using System.ComponentModel.DataAnnotations;

namespace ExpressYourself.Application.Models.IpAddresses;

public record IpAddressResponse
{
    public string? CountryName { get; set; }
    [Length(2,2)]
    public string? TwoLetterCode { get; set; }
    [Length(3,3)]
    public string? ThreeLetterCode { get; set; }

}