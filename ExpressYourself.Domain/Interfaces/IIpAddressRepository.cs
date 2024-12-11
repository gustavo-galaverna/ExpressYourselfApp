using ExpressYourself.Domain.Entities;

namespace ExpressYourself.Domain.Interfaces;

public interface IIpAddressRepository
{
    Task<IpAddress> CreateIpAddressAsync(IpAddress ipAddress);
    Task<IpAddress> UpdateIpAddressAsync(IpAddress ipAddress);
    
}