using AutoMapper;
using ExpressYourself.Application.Interfaces;
using ExpressYourself.Application.Models.IpAddresses;
using ExpressYourself.Application.Services;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Exceptions;
using ExpressYourself.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

public class IpAddressServiceTests
{
    private readonly Mock<IIpAddressRepository> _ipRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<IIP2CService> _ip2CServiceMock;
    private readonly Mock<IIpCountryReportRepository> _reportRepositoryMock;
    private readonly Mock<ICountryService> _countryServiceMock;
    private readonly IpAddressService _service;

    public IpAddressServiceTests()
    {
        _ipRepositoryMock = new Mock<IIpAddressRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheMock = new Mock<IMemoryCache>();
        _ip2CServiceMock = new Mock<IIP2CService>();
        _reportRepositoryMock = new Mock<IIpCountryReportRepository>();
        _countryServiceMock = new Mock<ICountryService>();

        _service = new IpAddressService(
            _ipRepositoryMock.Object,
            _mapperMock.Object,
            _cacheMock.Object,
            _ip2CServiceMock.Object,
            _reportRepositoryMock.Object,
            _countryServiceMock.Object
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

        // Mock TryGetValue behavior
        _cacheMock.Setup(c => c.TryGetValue("ips", out It.Ref<object?>.IsAny))
                  .Callback(new TryGetValueCallback((object key, out object? value) =>
                  {
                      value = cachedResponse; // Assign the cached data
                  }))
                  .Returns(true);

        // Act
        var result = await _service.GetIpDetails(ip);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ip, result.IpAddress);
        _cacheMock.Verify(c => c.TryGetValue("ips", out It.Ref<object?>.IsAny), Times.Once);
    }

    // Custom delegate for TryGetValue simulation
    private delegate void TryGetValueCallback(object key, out object? value);

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
}
