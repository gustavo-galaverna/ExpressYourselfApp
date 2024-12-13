using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpressYourself.Infrastructure.Persistence.Repository;

public class CountryRepository(ExpressYourselfContext expressYourselfContext) : ICountryRepository
{
     private readonly ExpressYourselfContext _context = expressYourselfContext ?? throw new ArgumentNullException(nameof(expressYourselfContext));

    public async Task<Country> CreateCountryAsync(Country country)
    {
        _context.Countries.Add(country);

        await _context.SaveChangesAsync();

        return country;
    }

    public async Task<Country> GetCountryByNameAsync(string twoLetterCode, string threeLetterCode)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(c => c.TwoLetterCode.ToLower().Equals(twoLetterCode.ToLower()) 
                                                                    && c.ThreeLetterCode.ToLower().Equals(threeLetterCode));

        return country!;
    }
}