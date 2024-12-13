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
        CreateMap<IpAddress, IpDetailResponse>()
            .ForMember(x => x.IpAddress, opt => opt.MapFrom(u => u.Ip))
            .ForMember(x => x.CountryName, opt => opt.MapFrom(u => u.Country.Name))
            .ForMember(x => x.TwoLetterCode, opt => opt.MapFrom(u => u.Country.TwoLetterCode))
            .ForMember(x => x.ThreeLetterCode, opt => opt.MapFrom(u => u.Country.ThreeLetterCode));
        CreateMap<IpDetails, IpDetailResponse>();
        CreateMap<IpCountryReport, IpCountryReportResponse>();
    }

}
