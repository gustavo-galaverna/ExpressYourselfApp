using AutoMapper;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.IpAddresses;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Exceptions;
using ExpressYourself.Domain.Interfaces;
using Moq;
using FluentAssertions;
using ExpressYourself.Application.Services;

public class IpAddressServiceTests
{
    private readonly Mock<IIpAddressRepository> _ipRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IIP2CService> _ip2CServiceMock;
    private readonly Mock<IIpCountryReportRepository> _ipCountryReportRepositoryMock;
    private readonly Mock<ICountryService> _countryServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly IpAddressService _service;

    public IpAddressServiceTests()
    {
        _ipRepositoryMock = new Mock<IIpAddressRepository>();
        _mapperMock = new Mock<IMapper>();
        _ip2CServiceMock = new Mock<IIP2CService>();
        _ipCountryReportRepositoryMock = new Mock<IIpCountryReportRepository>();
        _countryServiceMock = new Mock<ICountryService>();
        _cacheServiceMock = new Mock<ICacheService>();

        _service = new IpAddressService(
            _ipRepositoryMock.Object,
            _mapperMock.Object,
            _ip2CServiceMock.Object,
            _ipCountryReportRepositoryMock.Object,
            _countryServiceMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task GetIpDetails_ShouldReturnFromCache()
    {
        // Arrange
        var ip = "192.168.0.1";
        var cachedResponse = new List<IpDetailResponse>
        {
            new IpDetailResponse
            {
                IpAddress = ip,
                CountryName = "Test Country",
                TwoLetterCode = "TC",
                ThreeLetterCode = "TCO"
            }
        };

        _cacheServiceMock.Setup(c => c.GetAsync<List<IpDetailResponse>>("ips"))
            .ReturnsAsync(cachedResponse);

        // Act
        var result = await _service.GetIpDetails(ip);

        // Assert
        result.Should().NotBeNull();
        result.IpAddress.Should().Be(ip);
        _cacheServiceMock.Verify(c => c.GetAsync<List<IpDetailResponse>>("ips"), Times.Once);
    }

    [Fact]
    public async Task CreateIpAddressAsync_ShouldThrow_WhenInvalidIp()
    {
        // Arrange
        var invalidRequest = new IpDetails { IpAddress = "invalid-ip" };

        // Act
        Func<Task> action = () => _service.CreateIpAddressAsync(invalidRequest);

        // Assert
        await action.Should().ThrowAsync<InvalidIpException>()
            .WithMessage("Invalid IP Address.");
    }

    [Fact]
    public async Task GetCountryByIpAsync_ShouldReturnCorrectData()
    {
        // Arrange
        var countryCodes = new[] { "TC", "US" };
        var countryList = new List<IpCountryReport>
        {
            new IpCountryReport { AddressesCount = 1, CountryName = "Brazil", LastAddressUpdated = DateTime.Now},
            new IpCountryReport { AddressesCount = 2, CountryName = "France", LastAddressUpdated = DateTime.Now }
        };

        _ipCountryReportRepositoryMock.Setup(r => r.GetCountryByIpAsync(It.IsAny<string>()))
            .ReturnsAsync(countryList);

        // Mocking the mapper to map the response
        _mapperMock.Setup(m => m.Map<List<IpCountryReportResponse>>(It.IsAny<List<IpCountryReport>>()))
            .Returns(new List<IpCountryReportResponse>
            {
                new IpCountryReportResponse { AddressesCount = 1, CountryName = "Brazil", LastAddressUpdated = DateTime.Now},
                new IpCountryReportResponse { AddressesCount = 2, CountryName = "France", LastAddressUpdated = DateTime.Now }
            });

        // Act
        var result = await _service.GetCountryByIpAsync(countryCodes);

        // Assert
        result.Should().HaveCount(2);
        result[0].AddressesCount.Should().Be(1);
        result[1].AddressesCount.Should().Be(2);
    }
}
