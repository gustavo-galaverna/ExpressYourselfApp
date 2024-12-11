using AutoMapper;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.IpAddresses;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;

namespace ExpressYourself.Application.Services;

public class IpAddressService(IIpAddressRepository ipAddressRepository, IMapper mapper) : IIpAddressService
{
    private readonly IIpAddressRepository _ipRepository = ipAddressRepository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateIpAddressAsync(IpAddressRequest request)
    {
        IpAddress ipAddress;

        if(string.IsNullOrEmpty(request.Ip))
        {
            throw new InvalidDataException("Invalid country two letter code.");
        }

        try
        {
            ipAddress = _mapper.Map<IpAddress>(request);
            if(ipAddress is null)
            {
                throw new Exception("Ip Address initialization has failed.");
            }

            ipAddress = await _ipRepository.CreateIpAddressAsync(ipAddress);

        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task UpdateIpAddressAsync(IpAddressRequest request)
    {
        IpAddress ipAddress;

        try
        {
            ipAddress = _mapper.Map<IpAddress>(request);
            if(ipAddress is null)
            {
                throw new Exception("Ip Address initialization has failed.");
            }

            ipAddress = await _ipRepository.UpdateIpAddressAsync(ipAddress);

        }
        catch(Exception)
        {
            throw;
        }
    }
}