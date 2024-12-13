using ExpressYourself.Domain.Entities;

namespace ExpressYourself.Domain.Interfaces;

public interface IIP2CService
{
    Task<IpDetails> GetIpInformationAsync(string ipAddress);
}

