using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.IpAddresses;
using Microsoft.AspNetCore.Mvc;

namespace ExpressYourself.API.Controllers;

[Route("api/ip")]
[ApiController]
public class IpController(IIpAddressService ipAddressService) : ControllerBase
{
    private readonly IIpAddressService _ipAddressService = ipAddressService;

    /// <summary>
    /// Returns details of the giving IP address
    /// </summary>
    /// <remarks>
    /// Request example:
    /// ```
    /// GET  /api/ip/details
    /// </remarks>
    [HttpGet("details")]
    public async Task<ActionResult<IpDetailResponse>> IpDetails(string ip)
    {
        var response = await _ipAddressService.GetIpDetails(ip);
        if(response is null)
        {
            return BadRequest("IP details not found.");
        }

        return Ok(response);
    }


    /// <summary>
    /// Returns a report of the giving countries or all countries in case country codes are null
    /// </summary>
    /// <remarks>
    /// Request example:
    /// ```
    /// POST  /api/ip/report
    /// [
    ///     "BR", "JP"
    /// ]
    /// </remarks>
    [HttpPost("report")]
    public async Task<ActionResult<List<IpCountryReportResponse>>> CountriesReport(string[]? countryCodes)
    {
        var response = await _ipAddressService.GetCountryByIpAsync(countryCodes!);
        if(response is null || response.Count <=0)
        {
            return BadRequest("No IP's found for the giving country codes.");
        }

        return Ok(response);       
    }
}