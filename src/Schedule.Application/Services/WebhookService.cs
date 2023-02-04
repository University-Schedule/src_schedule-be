using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Schedule.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Schedule.Services;

public class WebhookService : IHostedService
{
    private readonly ILogger<WebhookService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly BotConfiguration _botConfig;

    public WebhookService(
        ILogger<WebhookService> logger,
        IServiceProvider serviceProvider,
        IOptions<BotConfiguration> botOptions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _botConfig = botOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhookAddress = $"{_botConfig.HostAddress}{_botConfig.Route}";
        _logger.LogInformation("Setting webhook: {WebhookAddress}", webhookAddress);
        await botClient.SetWebhookAsync(
            url: webhookAddress,
            allowedUpdates: Array.Empty<UpdateType>(),
            secretToken: _botConfig.SecretToken,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        _logger.LogInformation("Removing webhook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}