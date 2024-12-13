using System.ComponentModel.DataAnnotations;

namespace ExpressYourself.Application.Models.Countries;

public record CreateCountryRequest
{
    [Length(1,50)]
    public string? CountryName { get; set; }
    [Length(2,2)]
    public string? TwoLetterCode { get; set; }
    [Length(3,3)]
    public string? ThreeLetterCode { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}