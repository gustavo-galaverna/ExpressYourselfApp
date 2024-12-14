using Dapper;
using ExpressYourself.Domain.Entities;
using ExpressYourself.Domain.Interfaces;

namespace ExpressYourself.Infrastructure.Persistence.Repository;
public class IpCountryReportRepository(DatabaseConnection databaseConnection) : IIpCountryReportRepository
{
    private readonly DatabaseConnection _databaseConnection = databaseConnection;

    public async Task<List<IpCountryReport>> GetCountryByIpAsync(string countryCodes)
    {
        string whereClause = !string.IsNullOrEmpty(countryCodes) ? $"Where c.TwoLetterCode in ({countryCodes})" : "";
        using var connection = _databaseConnection.CreateConnection();

        const string baseQuery = @"
                    Select    c.Name as CountryName
                            , COUNT(i.Id) as AddressesCount
                            , MAX(i.UpdatedAt) as LastAddressUpdated 
                    From IpAddresses as i
                    Inner Join Countries as c on c.Id = i.CountryId
                    {0}
                    Group By c.Name";

        string query = string.Format(baseQuery, whereClause);
        var result = await connection.QueryAsync<IpCountryReport>(query);
        return result.ToList();
    }
}
