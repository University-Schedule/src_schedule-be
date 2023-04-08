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

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<MainMenuAnswerHandler>();
serviceCollection.AddScoped<RulesAnswerHandler>();
serviceCollection.AddScoped<TestAnswerHandler>();
serviceCollection.AddScoped<PaginationExampleAnswerHandler>();
serviceCollection.AddScoped<HelloAnswerHandler>();

var serviceProvider = serviceCollection.BuildServiceProvider();
var memoryCache = new MemoryCache(new MemoryCacheOptions());

//http://t.me/net_framework_bot
var botClient = new TelegramBotClient("6114166319:AAEokZNZlR3EuQ7SoYQKuJdGzzUUqf_Alno");

await StartAsync(x =>
{
    x.Default<MainMenuAnswerHandler>();
    x.Command<MainMenuAnswerHandler>("menu");
    x.Text<HelloAnswerHandler>("hi", "hello");
});

Console.ReadLine();



//BOT FRAMEWORK
Task StartAsync(Action<BotActionConfiguration> configure, CancellationToken cancellationToken = default)
{
    var configuration = new BotActionConfiguration();
    configure(configuration);

    botClient.StartReceiving(
        updateHandler: async (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) => await HandleMessageAsync(configuration, update?.Message),
        pollingErrorHandler: (ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask,
        receiverOptions: new() { AllowedUpdates = Array.Empty<UpdateType>() },
        cancellationToken: cancellationToken
    );
    return Task.CompletedTask;
}

async Task HandleMessageAsync(BotActionConfiguration configuration, Message message)
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
        await HandleType(scope, currentType!, payload, answerContext);
    }

    scope.Dispose();
}

(Type, object) ResolveCurrentType(AnswerContext answerContext, string messageText, BotActionConfiguration configuration)
{
    Type currentType = null;
    object payload = null;

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
        if (memoryCache.TryGetValue(answerContext.UserId, out Type resolvedType))
        {
            memoryCache.Remove(answerContext.UserId);
            currentType = resolvedType!;
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

async Task HandleType(IServiceScope scope, Type currentType, object payload, AnswerContext context)
{
    if (scope.ServiceProvider.GetService(currentType) is BaseAnswerHandler currentHandler)
    {
        currentHandler.AnswerContext = context;
        IAnswerResult? result = null;

        //resolve answer handler
        if (currentHandler is AnswerHandler answerHandler)
        {
            result = await answerHandler.HandleAsync(new MessageContext());
        }
        if (currentHandler is not AnswerHandler)
        {
            //TODO: refactor IQuery instead object
            result = await (Task<IAnswerResult?>)currentHandler.GetType().GetMethod(nameof(AnswerQueryHandler<object>.HandleAsync))
                .Invoke(currentHandler, new object[2] { new MessageContext(), payload });
        }

        //handle result
        if (result is RedirectTo redirectTo)
        {
            await HandleType(scope, redirectTo.RedirectType, redirectTo.Payload, context);
        }
        else if (result is RedirectReceiving redirectReceiving)
        {
            memoryCache.Set(context.UserId, redirectReceiving.RedirectType);
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

public sealed class RedirectionPayload
{
    public Type Type { get; set; }
    public object Payload { get; set; }
}