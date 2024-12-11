using System.ComponentModel.DataAnnotations;

namespace ExpressYourself.Domain.Entities;

public partial class IpAddress
{
    [Key]    
    public int Id { get; set; }
    [Required]
    public int CountryId { get; set; }
    [Required]
    [Length(1,15)]
    public string Ip { get; set; } = null!;
    [Required]    
    public DateTime CreatedAt { get; set; }

    [Required]    
    public DateTime UpdatedAt { get; set; }
    public virtual Country Country { get; set; } = null!;
}
