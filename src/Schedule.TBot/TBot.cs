using Schedule.TBot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Schedule.TBot
{
    public sealed class TBot
    {
        private readonly ITelegramBotClient _client;
        public TBot(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token not provided");
            }
            _client = new TelegramBotClient(token);
        }

        /// <summary>
        /// Processing bot updates use getUpdates (pull mechanism);
        /// If we want to use webhooks invoke RunWebhookAsync method;
        /// </summary>
        /// <returns></returns>
        public Task RunAsync(Action<BotActionConfiguration> configure, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            var configuration = new BotActionConfiguration();
            configure(configuration);

            _client.StartReceiving(
                updateHandler: async (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) => await HandleMessageAsync(serviceProvider, botClient, configuration, update.Message!),
                pollingErrorHandler: (ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask,
                receiverOptions: new() { AllowedUpdates = Array.Empty<UpdateType>() },
                cancellationToken: cancellationToken
            );
            return Task.CompletedTask;
        }

        //TODO: RunWebhookAsync()

        private async static Task HandleMessageAsync(IServiceProvider serviceProvider, ITelegramBotClient botClient, BotActionConfiguration configuration, Message message)
        {
            try
            {
                if (message is not null)
                {
                    var scope = serviceProvider.CreateScope();

                    var handler = new MessageHandler(botClient, configuration, scope);
                    await handler.HandleAsync(message);

                    scope.Dispose();
                }
            }
            catch { }
        }
    }
}