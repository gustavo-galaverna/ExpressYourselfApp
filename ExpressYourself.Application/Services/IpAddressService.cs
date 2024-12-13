using System.Net;
using AutoMapper;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.IpAddresses;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Exceptions;
using ExpressYourself.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ExpressYourself.Application.Services;

public class IpAddressService(IIpAddressRepository ipAddressRepository, IMapper mapper, 
                                IMemoryCache cache, IIP2CService iP2CService,
                                IIpCountryReportRepository ipCountryReportRepository, ICountryService countryService) : IIpAddressService
{
    private readonly IIpAddressRepository _ipRepository = ipAddressRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;
    private readonly IIP2CService _iP2CService = iP2CService;
    private readonly IIpCountryReportRepository _ipCountryReportRepository = ipCountryReportRepository;
    private readonly ICountryService _countryService = countryService;

    public async Task<IpDetailResponse> CreateIpAddressAsync(IpDetails request)
    {

        if(string.IsNullOrEmpty(request.IpAddress) || !IPAddress.TryParse(request.IpAddress, out _))
        {
            throw new InvalidIpException("Invalid IP Address.");
        }
        else if(string.IsNullOrEmpty(request.TwoLetterCode) || request.TwoLetterCode.Length > 2)
        {
            throw new InvalidCountryException("Invalid country two letter code.");
        }
        else if(string.IsNullOrEmpty(request.ThreeLetterCode) || request.ThreeLetterCode.Length > 3)
        {
            throw new InvalidCountryException("Invalid country three letter code.");
        }

        try
        {

            var country = await _countryService.GetOrCreateCountry(new(){
                CountryName = request.CountryName,
                TwoLetterCode = request.TwoLetterCode,
                ThreeLetterCode = request.ThreeLetterCode,
                CreatedAt = DateTime.UtcNow
            });

            IpAddress newIp = new()
            {
                CountryId = country.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Ip = request.IpAddress!
            };

            newIp =  await _ipRepository.CreateIpAddressAsync(newIp);

            var ipResponse = _mapper.Map<IpDetailResponse>(newIp);


            return ipResponse;

        }catch(Exception)
        {
            throw;
        }
    }

    public async Task<List<IpCountryReportResponse>> GetCountryByIpAsync(string[] countryCodes)
    {
        string countryCodesString = (countryCodes is null)? string.Empty : string.Join(",", countryCodes.Select(code => $"'{code}'"));

        var countryList = await _ipCountryReportRepository.GetCountryByIpAsync(countryCodesString);
        var response = _mapper.Map<List<IpCountryReportResponse>>(countryList);
        return response;
    }

    public async Task<IpDetailResponse> GetIpDetails(string ip)
    {
        _cache.TryGetValue("ips", out List<IpDetailResponse>? ips);
        if(ips is not null)
        {
             var cachedIp = ips.FirstOrDefault(i => i.IpAddress == ip);
             if(cachedIp is not null)  return cachedIp;
        }
        else
        {
            ips = [];
        }


        var dbIp = await _ipRepository.GetIpAddressAsync(ip);
        var ipDetails = _mapper.Map<IpDetailResponse>(dbIp);
        if (ipDetails != null)
        {
            ips.Add(ipDetails);
            _cache.Set("ips", ips, TimeSpan.FromHours(5));
            return ipDetails;
        }

        var externalData = await _iP2CService.GetIpInformationAsync(ip);
        await CreateIpAddressAsync(externalData);

        ipDetails = _mapper.Map<IpDetailResponse>(externalData);
        ips.Add(ipDetails);


        _cache.Set("ips", ips, TimeSpan.FromHours(5));

        return ipDetails;

    }

}