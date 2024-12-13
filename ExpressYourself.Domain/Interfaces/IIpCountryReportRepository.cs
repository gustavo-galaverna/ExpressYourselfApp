using ExpressYourself.Domain.Entities;

namespace ExpressYourself.Domain.Interfaces;

public interface IIpCountryReportRepository
{
    public Task<List<IpCountryReport>> GetCountryByIpAsync(string countryCodes);
}

