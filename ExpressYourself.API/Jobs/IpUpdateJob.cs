using ExpressYourself.Application;

namespace ExpressYourself.API.Jobs;

public class IpUpdateJob(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var ipUpdateService = scope.ServiceProvider.GetRequiredService<UpdateIpInformationService>();

                await ipUpdateService.UpdateIpInformationAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating IP information." + ex.Message);
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
