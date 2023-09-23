using System.Text;
using Driver.InfoCar.Pages;
using Driver.Options;
using Driver.Telegram;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Driver.InfoCar;

public sealed class CheckPracticalExamTermsService : BackgroundService
{
    private const string RegisterLink = "https://info-car.pl/new/prawo-jazdy/zapisz-sie-na-egzamin-na-prawo-jazdy/dane-osobowe";
    
    private readonly ITelegramService _telegramService;
    private readonly ApplicationOptions _options;

    public CheckPracticalExamTermsService(ITelegramService telegramService, IOptions<ApplicationOptions> options)
    {
        _telegramService = telegramService;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckPracticalExamTermsAsync();
            await Task.Delay(180000, stoppingToken);
        }
    }

    private async Task CheckPracticalExamTermsAsync()
    {
        var isAuthorized = ProfilePage.CheckIfAuthorized();
        if (!isAuthorized)
        {
            AuthorizationPage.Authorize(_options.Username, _options.Password);
        }

        var examDates = SchedulePage.FindNearestPracticalExamDates();
        foreach (var date in examDates)
        {
            if (date <= DateTime.Today.AddDays(_options.SearchPeriodInDays))
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Wanna drive with me at {date:MMMM dd}?");
                sb.AppendLine($"Link to register {RegisterLink}");
                    
                await _telegramService.SendAsync(sb.ToString());    
            }
        }
    }
}