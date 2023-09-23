namespace Driver.Telegram;

public interface ITelegramService
{
    Task SendAsync(string message);
}