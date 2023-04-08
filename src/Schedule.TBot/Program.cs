using Microsoft.Extensions.DependencyInjection;
using Schedule.TBot.Answers;
using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types.ReplyMarkups;
using Schedule.TBot.Framework.Keyboard;
using Schedule.TBot.Framework.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Schedule.TBot.Utils;
using Schedule.TBot.Answers.TextAnswers;
using Schedule.TBot.Answers.CommandAnswers;
using Schedule.TBot.Extensions;

var serviceCollection = new ServiceCollection();
serviceCollection.RegisterAnswerHandlers();

var serviceProvider = serviceCollection.BuildServiceProvider();
var memoryCache = new MemoryCache(new MemoryCacheOptions());


await StartAsync(x =>
{
    x.SetToken("6114166319:AAEokZNZlR3EuQ7SoYQKuJdGzzUUqf_Alno"); //http://t.me/net_framework_bot
    x.Default<MenuAnswerHandler>();
    x.Command<MenuAnswerHandler>("menu");
    x.Command<RulesAnswerHandler>("rules", "rule", "правила");
    x.Text<HelloExampleAnswerHandler>("hi", "hello");
});

Console.ReadLine();


//BOT FRAMEWORK
Task StartAsync(Action<BotActionConfiguration> configure, CancellationToken cancellationToken = default)
{
    var configuration = new BotActionConfiguration();
    configure(configuration);

    if (string.IsNullOrEmpty(configuration.Token))
    {
        throw new ArgumentException("Token not provided");
    }

    var botClient = new TelegramBotClient(configuration.Token);

    botClient.StartReceiving(
        updateHandler: async (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) => await HandleMessageAsync(botClient, configuration, update?.Message),
        pollingErrorHandler: (ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask,
        receiverOptions: new() { AllowedUpdates = Array.Empty<UpdateType>() },
        cancellationToken: cancellationToken
    );
    return Task.CompletedTask;
}

async Task HandleMessageAsync(ITelegramBotClient botClient, BotActionConfiguration configuration, Message message)
{
    var scope = serviceProvider.CreateScope();

    if (message is not null)
    {
        var answerContext = new AnswerContext {
            UserId = message.From.Id,
            RegisterReplyKeyboardAsync = RegisterReplyKeyboardAsync,
            BotClient = botClient
        };

        var (currentType, payload) = ResolveCurrentType(answerContext, message.Text, configuration);
        await HandleTypeAsync(scope, currentType!, payload, answerContext, message);
    }

    scope.Dispose();
}

(Type, IPayload) ResolveCurrentType(AnswerContext answerContext, string messageText, BotActionConfiguration configuration)
{
    Type currentType = null;
    IPayload payload = null;

    //check if user click on KeyboardActionButton
    if (memoryCache.TryGetValue($"{answerContext.UserId}_KEYBOARD", out Dictionary<string, RedirectionPayload> buttonsRedirections))
    {
        memoryCache.Remove($"{answerContext.UserId}_KEYBOARD");
        buttonsRedirections.TryGetValue(messageText, out var resolvedAction);
        if (resolvedAction != null)
        {
            currentType = resolvedAction.Type;
            payload = resolvedAction.Payload;
        }
    }

    if (currentType is null)
    {
        //check if user has RedirectReceiving
        if (memoryCache.TryGetValue(answerContext.UserId, out RedirectionPayload resolvedType))
        {
            memoryCache.Remove(answerContext.UserId);
            currentType = resolvedType.Type!;
            payload = resolvedType.Payload; 
        }
        //defult
        else
        {
            if (messageText.StartsWith('/') && configuration.GetCommandHandler(messageText) is not null)
            {
                currentType = configuration.GetCommandHandler(messageText);
            }
            else if (configuration.GetTextHandler(messageText) is not null)
            {
                currentType = configuration.GetTextHandler(messageText);
            } 
            else
            {
                currentType = configuration.DefaultHandler;
            }
        }
    }

    return (currentType, payload);
}

async Task HandleTypeAsync(IServiceScope scope, Type currentType, IPayload payload, AnswerContext context, Message message)
{
    if (scope.ServiceProvider.GetService(currentType) is BaseAnswerHandler currentHandler)
    {
        currentHandler.AnswerContext = context;
        var messageContext = new MessageContext() { Text = message.Text, UserId = context.UserId };

        IAnswerResult? result = currentHandler switch
        {
            AnswerHandler handler => await handler.HandleAsync(messageContext),
            _ when TypeUtils.IsSubclassOfRawGeneric(typeof(AnswerPayloadHandler<>), currentType) => 
                await (Task<IAnswerResult?>)currentHandler.GetType().GetMethod(nameof(AnswerPayloadHandler<IPayload>.HandleAsync)).Invoke(currentHandler, new object[] { messageContext, payload }),
            _ => new OkResult()
        };

        //handle result
        if (result is RedirectTo redirectTo)
        {
            await HandleTypeAsync(scope, redirectTo.RedirectType, redirectTo.Payload, context, message);
        }
        else if (result is RedirectReceiving redirectReceiving)
        {
            memoryCache.Set(context.UserId, new RedirectionPayload { Type = redirectReceiving.RedirectType, Payload = redirectReceiving.Payload });
        }
    }
}

async Task RegisterReplyKeyboardAsync(long userId, ReplyKeyboardMarkup replyKeyboard)
{
    var buttonsRedirections = new Dictionary<string, RedirectionPayload>();
    foreach (var row in replyKeyboard.Keyboard)
    {
        foreach (var button in row)
        {
            if (button is KeyboardActionButton actionButton && actionButton.Action is RedirectTo redirectTo)
            {
                buttonsRedirections.Add(button.Text, new RedirectionPayload { Type = redirectTo.RedirectType, Payload = redirectTo.Payload });
            }
        }
    }
    memoryCache.Set($"{userId}_KEYBOARD", buttonsRedirections);
}

