using System.ComponentModel.DataAnnotations;

namespace ExpressYourself.Application.Models.IpAddresses;

public record IpAddressRequest
{
    public int? Id { get; set; }
    public int CountryId { get; set; }
    public string Ip { get; set; } = null!;  
    public DateTime CreatedAt { get; set; } = DateTime.Now;
 
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

}