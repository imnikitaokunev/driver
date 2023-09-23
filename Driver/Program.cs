using Driver.InfoCar;
using Driver.Options;
using Driver.Telegram;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;

namespace Driver;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();

        Browser.Initialize();

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.Sources.Clear();

                var environment = hostingContext.HostingEnvironment.EnvironmentName;
                configuration.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{environment}.json", true, true)
                    .AddEnvironmentVariables()
                    .Build();
            })
            .UseSerilog((context, configuration) =>
            {
                configuration.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .WriteTo.Console()
                    .WriteTo.File("log.log")
                    .ReadFrom.Configuration(context.Configuration);
            })
            .ConfigureServices((context, services) =>
            {
                var configurationRoot = context.Configuration;
                
                services.Configure<ApplicationOptions>(
                    configurationRoot.GetSection(ApplicationOptions.Application));

                services.AddSingleton<ActiveChatsStorage>();
                services.AddTransient<ITelegramService, TelegramService>();
                services.AddTransient<ITelegramBotClient>(_ =>
                    new TelegramBotClient(configurationRoot.GetValue<string>("Telegram:ApiToken")));

                services.AddHostedService<TelegramActiveChatsService>();
                services.AddHostedService<CheckPracticalExamTermsService>();
            });
    }
}