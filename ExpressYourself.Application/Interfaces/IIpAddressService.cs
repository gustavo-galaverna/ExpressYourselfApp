using ExpressYourself.Application.Models;
using ExpressYourself.Application.Models.IpAddresses;

namespace ExpressYourself.Application.Interfaces;

public interface IIpAddressService
{
    Task CreateIpAddressAsync(IpAddressRequest request);
    Task UpdateIpAddressAsync(IpAddressRequest request);
    
}