using Apps.Demo.Lottery.Application.Contracts;
using Apps.Demo.Lottery.Application.Services;
using Apps.Demo.Lottery.Domain.Configurations;
using Apps.Demo.Lottery.Domain.Contracts;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Contracts.PrizeDistributors;
using Apps.Demo.Lottery.Domain.Services;
using Apps.Demo.Lottery.Domain.Services.PrizeDeciders;
using Apps.Demo.Lottery.Domain.Services.PrizeDistributors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apps.Demo.Lottery;

internal static class ConfigureDi
{
    public static IServiceCollection ConfigureApplicationSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        var gameSettings = new GameSettings();
        configuration.GetSection("GameSettings").Bind(gameSettings);

        var ticketSettings = new TicketSettings();
        configuration.GetSection("TicketSettings").Bind(ticketSettings);

        var playerSettings = new PlayerSettings();
        configuration.GetSection("PlayerSettings").Bind(playerSettings);
        services.AddSingleton(gameSettings)
            .AddSingleton(ticketSettings)
            .AddSingleton(playerSettings);
        return services;
    }

    public static IServiceCollection ConfigurePrizeDistributors(this IServiceCollection services) =>
        services.AddKeyedTransient<IPrizeDistributionStrategy, GrandPrizeDistributionStrategy>("GrandTier")
            .AddKeyedTransient<IPrizeDistributionStrategy, SecondTierPrizeDistributionStrategy>("SecondTier")
            .AddKeyedTransient<IPrizeDistributionStrategy, ThirdTierPrizeDistributionStrategy>("ThirdTier");

    public static IServiceCollection ConfigurePrizeDeciders(this IServiceCollection services) =>
        services.AddTransient<IGrandPriceDecisionService, GrandPrizeDecisionService>()
            .AddTransient<ISecondTierPrizeDecisionService, SecondTierPrizeDecisionService>()
            .AddTransient<IThirdTierPrizeDecisionService, ThirdTierPrizeDecisionService>();

    public static IServiceCollection ConfigureTicketServices(this IServiceCollection services) =>
        services.AddTransient<ITicketPurchasesService, TicketPurchasesService>();


    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services) =>
        services.AddTransient<IPrizeDistributionOrchestrator, PrizeDistributionOrchestrator>()
            .AddTransient<IGameService, GameService>()
            .AddTransient<AppRunner>();
}