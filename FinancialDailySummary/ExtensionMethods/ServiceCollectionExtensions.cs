using Microsoft.Extensions.Configuration;

namespace FinancialDailySummary.ExtensionMethods;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureBotSettings(
        this IServiceCollection services,
        string botToken) 
    {
        services.Configure<BotSettings>(x => x.BotToken = botToken);
        services.PostConfigure<BotSettings>(config =>
        {
            if (string.IsNullOrEmpty(config.BotToken))
            {
                throw new ApplicationException("BotToken is required");
            }

        });
        return services;
    }
}

