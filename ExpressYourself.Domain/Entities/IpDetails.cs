namespace Domain.Entities
{
    public class IpDetails
    {
        public string? IpAddress { get; set; }
        public string? CountryName { get; set; }
        public string? TwoLetterCode { get; set; }
        public string? ThreeLetterCode { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
