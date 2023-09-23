using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Driver.Telegram;

public sealed class TelegramService : ITelegramService
{
    private readonly ActiveChatsStorage _activeChatsStorage;
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramService> _logger;

    public TelegramService(ITelegramBotClient botClient, ActiveChatsStorage activeChatsStorage, ILogger<TelegramService> logger)
    {
        _botClient = botClient;
        _logger = logger;
        _activeChatsStorage = activeChatsStorage;
    }

    public async Task SendAsync(string message)
    {
        foreach (var chatId in _activeChatsStorage)
        {
            try
            {
                await _botClient.SendTextMessageAsync(chatId, message, parseMode: ParseMode.Html,
                    disableWebPagePreview: true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while sending message in Telegram: {ex.Message}", ex);
            }
        }
    }
}