using ExpressYourself.Domain.Entities;

namespace ExpressYourself.Domain.Interfaces;

public interface ICountryRepository
{
    Task<Country> GetCountryByNameAsync(string twoLetterCode, string threeLetterCode);
    Task<Country> CreateCountryAsync(Country country);
}