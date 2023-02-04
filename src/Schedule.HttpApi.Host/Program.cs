using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Schedule.Extensions;
using Schedule.Handlers;
using Schedule.Services;
using Schedule.Settings;
using Serilog;
using Serilog.Events;
using Telegram.Bot;

namespace Schedule;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting Schedule.HttpApi.Host.");
            
            var builder = WebApplication.CreateBuilder(args);
            
            
            var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
            builder.Services.Configure<BotConfiguration>(botConfigurationSection);

            var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

            builder.Services
                .AddControllers()
                .AddNewtonsoftJson();
// Add services to the container.

            builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                    TelegramBotClientOptions options = new(botConfig.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

            builder.Services.AddScoped<BotHandler>();

            builder.Services.AddHostedService<WebhookService>();
            
            builder.Host
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<ScheduleHttpApiHostModule>();
            var app = builder.Build();
            
            //app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
