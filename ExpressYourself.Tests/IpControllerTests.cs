using ExpressYourself.API.Controllers;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.IpAddresses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ExpressYourself.Tests.Controllers;

public class IpControllerTests
{
    private readonly Mock<IIpAddressService> _ipAddressServiceMock;

    public IpControllerTests()
    {
        _ipAddressServiceMock = new Mock<IIpAddressService>();
    }

    [Fact]
    public async Task IpDetails_ShouldReturnOk_WhenIpDetailsFound()
    {
        // Arrange
        var ip = "192.168.0.1";
        var ipDetails = new IpDetailResponse
        {
            IpAddress = ip,
            CountryName = "Test Country",
            TwoLetterCode = "TC",
            ThreeLetterCode = "TCO",
        };

        _ipAddressServiceMock
            .Setup(service => service.GetIpDetails(ip))
            .ReturnsAsync(ipDetails);

        var controller = new IpController(_ipAddressServiceMock.Object);

        // Act
        var result = await controller.IpDetails(ip);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(ipDetails, okResult.Value);
    }

    [Fact]
    public async Task IpDetails_ShouldReturnBadRequest_WhenIpDetailsNotFound()
    {
        // Arrange
        var ip = "192.168.0.1";

        _ipAddressServiceMock
            .Setup(service => service.GetIpDetails(It.IsAny<string>()))
            .ReturnsAsync((IpDetailResponse?)null);

        var controller = new IpController(_ipAddressServiceMock.Object);

        // Act
        var result = await controller.IpDetails(ip);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("IP details not found.", badRequestResult.Value);
    }

    [Fact]
    public async Task CountriesReport_ShouldReturnOk_WhenCountriesFound()
    {
        // Arrange
        string[] countryCodes = { "TC", "US" };
        var reportResponse = new List<IpCountryReportResponse>
        {
            new() {
                CountryName = "Test Country",
                AddressesCount = 5,
                LastAddressUpdated = DateTime.UtcNow
            }
        };

        _ipAddressServiceMock
            .Setup(service => service.GetCountryByIpAsync(It.IsAny<string[]>()))
            .ReturnsAsync(reportResponse);

        var controller = new IpController(_ipAddressServiceMock.Object);

        // Act
        var result = await controller.CountriesReport(countryCodes);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.Equal(reportResponse, okResult.Value);
    }

    [Fact]
    public async Task CountriesReport_ShouldReturnBadRequest_WhenNoCountriesFound()
    {
        // Arrange
        string[] countryCodes = { "TC" };

        _ipAddressServiceMock
            .Setup(service => service.GetCountryByIpAsync(It.IsAny<string[]>()))
            .ReturnsAsync(new List<IpCountryReportResponse>());

        var controller = new IpController(_ipAddressServiceMock.Object);

        // Act
        var result = await controller.CountriesReport(countryCodes);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("No IP's found for the giving country codes.", badRequestResult.Value);
    }
}
