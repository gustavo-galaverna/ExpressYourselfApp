using AutoMapper;
using ExpressYourself.Application.Models.Countries;
using ExpressYourself.Application.Services;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
public class CountryServiceTests
{
    private readonly Mock<ICountryRepository> _countryRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CountryService _service;

    public CountryServiceTests()
    {
        _countryRepositoryMock = new Mock<ICountryRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new CountryService(_countryRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetOrCreateCountry_ShouldCreateCountry_WhenNotExists()
    {
        // Arrange
        var request = new CreateCountryRequest
        {
            CountryName = "Country A",
            TwoLetterCode = "CA",
            ThreeLetterCode = "CAN"
        };
        _countryRepositoryMock.Setup(r => r.GetCountryByNameAsync(request.TwoLetterCode, request.ThreeLetterCode))
            .ReturnsAsync((Country)null);

        var country = new Country { Id = 1, Name = "Country A" };
        _mapperMock.Setup(m => m.Map<Country>(request)).Returns(country);
        _countryRepositoryMock.Setup(r => r.CreateCountryAsync(country)).ReturnsAsync(country);

        // Act
        var result = await _service.GetOrCreateCountry(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Country A");
    }
}
