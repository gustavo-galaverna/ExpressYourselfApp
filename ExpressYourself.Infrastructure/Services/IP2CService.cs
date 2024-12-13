using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Exceptions;
using ExpressYourself.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ExpressYourself.Infrastructure.Services
{
    public class IP2CService(HttpClient httpClient, IConfiguration configuration) : IIP2CService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _configuration = configuration;

        public async Task<IpDetails> GetIpInformationAsync(string ipAddress)
        {
            try
            {
                string uri = _configuration["IP2C:Uri"]!.ToString();
                var response = await _httpClient.GetAsync($"{uri}{ipAddress}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidIpException($"Failed to retrieve IP information for {ipAddress}. StatusCode: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var parts = content.Split(';');
                if (parts.Length < 4 || parts[0] != "1")
                {
                    throw new InvalidIpException($"Invalid response format for IP {ipAddress}: {content}");
                }

                return new IpDetails
                {
                    IpAddress = ipAddress,
                    TwoLetterCode = parts[1],
                    ThreeLetterCode = parts[2],
                    CountryName = parts[3],
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception)
            {
                throw new Exception($"Error retrieving IP information for {ipAddress}");
            }
        }
    }
}
