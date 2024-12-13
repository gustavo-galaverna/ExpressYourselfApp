using ExpressYourself.Application.Models.Countries;
using ExpressYourself.Domain.Entities;

namespace ExpressYourself.Application.Interfaces;

public interface ICountryService
{
    Task<Country> GetOrCreateCountry(CreateCountryRequest request);   
}