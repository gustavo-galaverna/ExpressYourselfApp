using ExpressYourself.Domain.Entities;

namespace ExpressYourself.Domain.Interfaces;

public interface ICountryRepository
{
    Task<Country> CreateCountryAsync(Country country);
}