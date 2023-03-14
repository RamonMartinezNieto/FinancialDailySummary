using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using static FinancialDailySummary.Enums.CommandsEnum;

namespace FinancialDailySummary.Services;

internal class TelegramBot : ITelegramBot
{
    private readonly ILogger<TelegramBot> _logger;
    private readonly BotSettings _botSettings;
    private readonly ITelegramBotClient _botClient;
    private readonly YahooFinancialClient _financialClient;

    public TelegramBot(
        IOptions<BotSettings> botSettings,
        ILogger<TelegramBot> logger,
        YahooFinancialClient financialClient)
    {
        _logger = logger;
        _botSettings = botSettings.Value;
        _botClient = new TelegramBotClient(_botSettings.BotToken);
        _financialClient = financialClient;
    }


    public void StartBot()
    {
        using CancellationTokenSource cts = new();

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        _logger.LogInformation("Bot listening...");
    }


    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {

        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        _logger.LogInformation($"Received a '{messageText}' message in chat {message.Chat.Id}.");

        try { 
            if (IsValidCommand(messageText)) {
                Commands comand = ParseEnumFromDescription(messageText);

                var dataIndex = await _financialClient.GetDataIndex(comand, Intervals.ThirteenMin);

                //extract
                var labels = dataIndex.Chart.Result[0].Timestamp
                    .Select(x => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(x).ToString("HH:mm:ss"))
                    .ToArray();

                var data = dataIndex.Chart.Result[0].Indicators.Quote[0].Close
                    .Select(x => (int?)x ).ToArray();

                //todo need refactor
                var createImage = new ChartService(labels, data, comand);
                var shorturl = createImage.Chart.GetShortUrl();

                await SentImage(message.Chat.Id,
                    createImage.Chart.GetShortUrl(),
                    dataIndex.GetMessage(comand), 
                    cancellationToken);
                //await SentMessageAsync(message.Chat.Id, dataIndex.GetMessage(comand), cancellationToken);
            }
            else 
                await SentMessageAsync(message.Chat.Id, GetMessageListCommands("*Lista de comandos:*"), cancellationToken);
        }
        catch (Exception ex) 
        {
            _logger.LogCritical("Something was wrong", ex);
            await SentMessageAsync(message.Chat.Id, "Servicio no disponible, vuelva a intentarlo", cancellationToken);
            return;
        }
    }


    private async Task<Message> SentMessageAsync(
        ChatId chatId,
        string text, 
        CancellationToken cts)
        => await _botClient.SendTextMessageAsync(
                 chatId: chatId,
                 text: text,
                 disableNotification: true,
                 parseMode: ParseMode.MarkdownV2,
                 cancellationToken: cts);

    

    private async Task<Message> SentImage(
        ChatId chatId,
        string pathImage,
        string text, 
        CancellationToken cts)
        => await _botClient.SendPhotoAsync(
                 chatId: chatId,
                 photo: pathImage,
                 caption: text,
                 disableNotification: true,
                 parseMode: ParseMode.MarkdownV2,
                 cancellationToken: cts);

    

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogCritical(errorMessage);

        return Task.CompletedTask;
    }
}