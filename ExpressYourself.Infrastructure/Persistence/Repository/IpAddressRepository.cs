using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpressYourself.Infrastructure.Persistence.Repository;

public class IpAddressRepository : IIpAddressRepository
{
     private readonly ExpressYourselfContext _context;
    public IpAddressRepository(ExpressYourselfContext expressYourselfContext)
    {
        _context = expressYourselfContext ?? throw new ArgumentNullException(nameof(expressYourselfContext));
    }

    public async Task<IpAddress> CreateIpAddressAsync(IpAddress ipAddress)
    {
        await _context.Ipaddresses.AddAsync(ipAddress);

        await _context.SaveChangesAsync();
        
        throw new NotImplementedException();
    }

    public async Task<IpAddress> UpdateIpAddressAsync(IpAddress ipAddress)
    {
        var previousAddress = await _context.Ipaddresses.FirstOrDefaultAsync(i => i.Ip.Equals(ipAddress.Ip)) ?? throw new Exception("IP Address was not found.");

        previousAddress.CountryId = ipAddress.CountryId;
        previousAddress.UpdatedAt = ipAddress.UpdatedAt;

        _context.Ipaddresses.Update(previousAddress);
        await _context.SaveChangesAsync();

        return previousAddress;
    }
}