using FinancialDailySummary.Clients;

namespace FinancialDailySummary.Services;

internal class Application : IApplication
{
    private readonly ILogger<Application> _logger;
    private readonly ITelegramBot _telegramBot;

    public Application(
        ILogger<Application> logger,
        ITelegramBot telegramBot)
    {
        _logger = logger;
        _telegramBot = telegramBot;
    }
    

    public void Run()
    {
        _logger.LogInformation("Starting application...");
        _telegramBot.StartBot();
    }
}
