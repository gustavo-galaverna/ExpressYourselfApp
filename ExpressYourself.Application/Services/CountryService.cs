using AutoMapper;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.Countries;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;

namespace ExpressYourself.Application.Services;

public class CountryService(ICountryRepository countryRepository, IMapper mapper) : ICountryService
{
    private readonly ICountryRepository _countryRepository = countryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateCountryAsync(CreateCountryRequest request)
    {
        Country country;

        if(string.IsNullOrEmpty(request.TwoLetterCode) || request.TwoLetterCode.Length > 2)
        {
            throw new InvalidDataException("Invalid country two letter code.");
        }else if(string.IsNullOrEmpty(request.ThreeLetterCode) || request.ThreeLetterCode.Length > 3)
        {
            throw new InvalidDataException("Invalid country three letter code.");
        }else if(string.IsNullOrEmpty(request.CountryName))
        {
          throw new InvalidDataException("Invalid country name.");   
        }
        
        try
        {
            country = _mapper.Map<Country>(request);
            if(country is null)
            {
                throw new Exception("Country initialization has failed.");
            }

            country = await _countryRepository.CreateCountryAsync(country);

        }
        catch(Exception)
        {
            throw;
        }

    }
}