using FinancialDailySummary.Clients;

namespace FinancialDailySummary.Services;

internal class Application : IApplication
{
    private ILogger<Application> _logger;
    private ITelegramBot _telegramBot;
    private YahooFinancialClient _client;

    public Application(
        ILogger<Application> logger,
        ITelegramBot telegramBot,
        YahooFinancialClient client)
    {
        _logger = logger;
        _telegramBot = telegramBot;
        _client = client;
    }
    

    public void Run()
    {
        _logger.LogInformation("Starting application...");

        _telegramBot.StartBot();
    }
}
