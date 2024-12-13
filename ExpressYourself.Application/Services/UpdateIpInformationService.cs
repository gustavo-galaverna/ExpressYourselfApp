using Microsoft.Extensions.Caching.Memory;
using ExpressYourself.Domain.Interfaces;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Application.Models.IpAddresses;

namespace ExpressYourself.Application;

public class UpdateIpInformationService(IIpAddressRepository repository, 
                                        IIP2CService ip2cService, IMemoryCache cache)
{
    private readonly IIpAddressRepository _repository = repository;
    private readonly IIP2CService _ip2cService = ip2cService;
    private readonly IMemoryCache _cache = cache;


    public async Task UpdateIpInformationAsync()
    {
        const int batchSize = 100;
        List<IpAddress> batch;
        _cache.TryGetValue("ips", out List<IpDetailResponse>? cachedIps);
        int cachedCount = cachedIps?.Count ?? 0;
        do
        {
            batch = await _repository.GetIpAddressesInBatchesAsync(batchSize);

            foreach (var ipAddress in batch)
            {
                var updatedDetails = await _ip2cService.GetIpInformationAsync(ipAddress.Ip);

                if (ipAddress.Country.Name != updatedDetails.CountryName ||
                    ipAddress.Country.TwoLetterCode != updatedDetails.TwoLetterCode ||
                    ipAddress.Country.ThreeLetterCode != updatedDetails.ThreeLetterCode)
                {
                    ipAddress.Country.Name = updatedDetails.CountryName!;
                    ipAddress.Country.TwoLetterCode = updatedDetails.TwoLetterCode!;
                    ipAddress.Country.ThreeLetterCode = updatedDetails.ThreeLetterCode!;

                    await _repository.UpdateIpAddressAsync(ipAddress);

                    if(cachedIps?.Count > 0)
                    {
                        var invalidIp = cachedIps.FirstOrDefault(ip => ip.IpAddress!.Equals(ipAddress.Ip));
                        if (invalidIp is not null)  cachedIps.Remove(invalidIp);
                    }
                }
            }
        }
        while (batch.Count == batchSize);

        if(cachedIps is not null && cachedIps.Count != cachedCount)
            _cache.Set("ips", cachedIps);
    }
}
