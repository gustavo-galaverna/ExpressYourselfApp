using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;

namespace ExpressYourself.Infrastructure.Persistence.Repository;

public class CountryRepository(ExpressYourselfContext expressYourselfContext) : ICountryRepository
{
     private readonly ExpressYourselfContext _context = expressYourselfContext ?? throw new ArgumentNullException(nameof(expressYourselfContext));

    public async Task<Country> CreateCountryAsync(Country country)
    {
        await _context.Countries.AddAsync(country);

        await _context.SaveChangesAsync();

        return country;
    }
}