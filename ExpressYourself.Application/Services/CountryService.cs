using AutoMapper;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.Countries;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Exceptions;
using ExpressYourself.Domain.Interfaces;

namespace ExpressYourself.Application.Services;

public class CountryService(ICountryRepository countryRepository, IMapper mapper) : ICountryService
{
    private readonly ICountryRepository _countryRepository = countryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Country> GetOrCreateCountry(CreateCountryRequest request)
    {

        if(string.IsNullOrEmpty(request.TwoLetterCode) || request.TwoLetterCode.Length > 2)
        {
            throw new InvalidCountryException("Invalid country two letter code.");
        }else if(string.IsNullOrEmpty(request.ThreeLetterCode) || request.ThreeLetterCode.Length > 3)
        {
            throw new InvalidCountryException("Invalid country three letter code.");
        }else if(string.IsNullOrEmpty(request.CountryName))
        {
          throw new InvalidCountryException("Invalid country name.");   
        }

        var country = await _countryRepository.GetCountryByNameAsync(request.TwoLetterCode!, request.ThreeLetterCode!);
        if(country is null)
        {
            country = _mapper.Map<Country>(request);

            country = await _countryRepository.CreateCountryAsync(country);
        }

        if(country is null) throw new InvalidCountryException("Country could not be created.");

        return country;
    }
}