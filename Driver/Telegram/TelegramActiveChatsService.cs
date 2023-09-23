using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Driver.Telegram;

public sealed class TelegramActiveChatsService : BackgroundService
{
    private readonly ActiveChatsStorage _activeChatsStorage;
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramActiveChatsService> _logger;
    private readonly QueuedUpdateReceiver _updateReceiver;
    private readonly Dictionary<string, Action<long, ActiveChatsStorage>> _commandsList = new()
    {
        ["/start"] = (x, storage) => storage.Add(x),
        ["/stop"] = (x, storage) => storage.Remove(x)
    };

    public TelegramActiveChatsService(ITelegramBotClient botClient, ILogger<TelegramActiveChatsService> logger,
        ActiveChatsStorage activeChatsStorage)
    {
        _botClient = botClient;
        _updateReceiver = new QueuedUpdateReceiver(botClient);
        _activeChatsStorage = activeChatsStorage;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await foreach (var update in _updateReceiver.WithCancellation(stoppingToken))
            {
                if (update.Message is not { } message || string.IsNullOrEmpty(message.Text))
                {
                    continue;
                }

                if (_commandsList.TryGetValue(message.Text, out var handler))
                {
                    handler(message.Chat.Id, _activeChatsStorage);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(message.Chat.Id,
                        "Don't waste my time, only real race talk allowed here", cancellationToken: stoppingToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
    }
}