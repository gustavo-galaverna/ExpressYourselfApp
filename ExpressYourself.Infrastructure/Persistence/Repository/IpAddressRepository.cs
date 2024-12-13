using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpressYourself.Infrastructure.Persistence.Repository;

public class IpAddressRepository(ExpressYourselfContext expressYourselfContext) : IIpAddressRepository
{
    private readonly ExpressYourselfContext _context = expressYourselfContext ?? throw new ArgumentNullException(nameof(expressYourselfContext));

    public async Task<IpAddress> CreateIpAddressAsync(IpAddress ipAddress)
    {
        await _context.IpAddresses.AddAsync(ipAddress);

        await _context.SaveChangesAsync();
        
        return ipAddress;
    }

    public async Task<IpAddress> GetIpAddressAsync(string ipAddress)
    {
        var ipInfo = await _context.IpAddresses
            .Include(c => c.Country)
            .FirstOrDefaultAsync(i => i.Ip.Equals(ipAddress));

        return ipInfo!;
    }

    public async Task<List<IpAddress>> GetIpAddressesInBatchesAsync(int batchSize)
    {
        return await _context.IpAddresses
            .OrderBy(ip => ip.UpdatedAt)
            .Take(batchSize)
            .Include(ip => ip.Country)
            .ToListAsync();        
    }

    public async Task<IpAddress> UpdateIpAddressAsync(IpAddress ipAddress)
    {
        ipAddress.UpdatedAt = DateTime.UtcNow;

        if (ipAddress.Country is not null)
        {
            var updatedDetails = ipAddress.Country;
            var existingCountry = await _context.Countries
                .FirstOrDefaultAsync(c =>
                    c.TwoLetterCode == updatedDetails.TwoLetterCode &&
                    c.ThreeLetterCode == updatedDetails.ThreeLetterCode);

            if (existingCountry != null)
            {
                ipAddress.CountryId = existingCountry.Id;
            }
            else
            {
                var newCountry = new Country
                {
                    Name = updatedDetails.Name,
                    TwoLetterCode = updatedDetails.TwoLetterCode,
                    ThreeLetterCode = updatedDetails.ThreeLetterCode
                };

                _context.Countries.Add(newCountry);
                await _context.SaveChangesAsync();

                ipAddress.CountryId = newCountry.Id;

            }

        }

        _context.IpAddresses.Update(ipAddress);
        await _context.SaveChangesAsync();

        return ipAddress;

    }
}