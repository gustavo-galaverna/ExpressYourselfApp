
using System.ComponentModel.DataAnnotations;

namespace ExpressYourself.Domain.Entities;

public partial class Country
{
    [Key]    
    public int Id { get; set; }
    private string _name = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Name
    {
        get => _name;
        set => _name = value.Length > 50 ? value.Substring(0, 50) : value;
    }

    private string _twoLetterCode = string.Empty;
    [Required]
    [Length(2,2)]
    public string TwoLetterCode
    {
        get => _twoLetterCode;
        set => _twoLetterCode = value.Length > 2 ? value.Substring(0, 2) : value;
    }

    private string _threeLetterCode = string.Empty;
    [Required]
    [Length(3,3)]
    public string ThreeLetterCode
    {
        get => _threeLetterCode;
        set => _threeLetterCode = value.Length > 3 ? value.Substring(0, 3) : value;
    }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<IpAddress> IpAddresses { get; set; } = new List<IpAddress>();

}
