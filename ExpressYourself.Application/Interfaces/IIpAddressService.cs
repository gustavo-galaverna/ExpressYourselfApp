using ExpressYourself.Application.Models.IpAddresses;

namespace ExpressYourself.Application.Interfaces;

public interface IIpAddressService
{
    Task<IpDetailResponse> GetIpDetails(string ip);
    Task<List<IpCountryReportResponse>> GetCountryByIpAsync(string[] countryCodes);
    
}