using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Demo.Lottery;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection()
            .ConfigureApplicationSettings(configuration)
            .ConfigureTicketServices()
            .ConfigurePrizeDeciders()
            .ConfigurePrizeDistributors()
            .ConfigureApplicationServices();
        var provider = services.BuildServiceProvider();

        var appRunner = provider.GetService<AppRunner>();
        ArgumentNullException.ThrowIfNull(appRunner);
        appRunner.Run();
    }
}