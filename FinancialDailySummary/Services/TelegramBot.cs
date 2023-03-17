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
    private readonly IChartService _chartService;

    public TelegramBot(
        IOptions<BotSettings> botSettings,
        ILogger<TelegramBot> logger,
        IChartService chartService,
        YahooFinancialClient financialClient)
    {
        _logger = logger;
        _botSettings = botSettings.Value;
        _botClient = new TelegramBotClient(_botSettings.BotToken);
        _financialClient = financialClient;
        _chartService = chartService;
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
                Commands command = ParseEnumFromDescription(messageText);

                var dataIndex = await _financialClient.GetDataIndex(command, Intervals.ThirteenMin);
                
                var labels = dataIndex.GetLabels();
                var data = dataIndex.GetData();

                await SentImageAsync(message.Chat.Id,
                    _chartService.GetUrlChart(labels, data, command),
                    dataIndex.GetMessage(command), 
                    cancellationToken);
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

    

    private async Task<Message> SentImageAsync(
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