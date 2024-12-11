using ExpressYourself.Application.Models.Countries;

namespace ExpressYourself.Application.Interfaces;

public interface ICountryService
{
    Task CreateCountryAsync(CreateCountryRequest request);
    
}