
using System.ComponentModel.DataAnnotations;

namespace ExpressYourself.Infrastructure;

public partial class Country
{
    [Key]    
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    [Length(2,2)]
    public string TwoLetterCode { get; set; } = null!;
    [Required]
    [Length(3,3)]
    public string ThreeLetterCode { get; set; } = null!;
    [Required]
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<IpAddress> IpAddresses { get; set; } = new List<IpAddress>();
}
