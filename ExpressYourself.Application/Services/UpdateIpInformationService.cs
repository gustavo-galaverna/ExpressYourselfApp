using Microsoft.Extensions.Caching.Memory;
using ExpressYourself.Domain.Interfaces;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Application.Models.IpAddresses;

namespace ExpressYourself.Application;

public class UpdateIpInformationService(IIpAddressRepository repository, 
                                        IIP2CService ip2cService, ICacheService cacheService)
{
    private readonly IIpAddressRepository _repository = repository;
    private readonly IIP2CService _ip2cService = ip2cService;
    private readonly ICacheService _cacheService = cacheService;


    public async Task UpdateIpInformationAsync()
    {
        const int batchSize = 100;
        List<IpAddress> batch;
        List<IpDetailResponse>? cachedIps = await _cacheService.GetAsync<List<IpDetailResponse>>("ips");
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
                await _cacheService.SetAsync("ips", cachedIps, TimeSpan.FromHours(5));
    }
}
