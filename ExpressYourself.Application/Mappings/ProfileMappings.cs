using AutoMapper;
using ExpressYourself.Application.Models.Countries;
using ExpressYourself.Application.Models.IpAddresses;
using ExpressYourself.Domain.Entities;

namespace ExpressYourself.Application.Mappings;

public class ProfileMappings : Profile
{
    public ProfileMappings()
    {
        CreateMap<IpAddressRequest, IpAddress>();
        CreateMap<CreateCountryRequest, Country>();
    }

}
