using Microsoft.Extensions.DependencyInjection;
using Schedule.TBot.Answers;
using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types.ReplyMarkups;
using Schedule.TBot.Framework.Keyboard;
using Schedule.TBot.Framework.Handlers;

var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<MainMenuAnswerHandler>();
serviceCollection.AddScoped<RulesAnswerHandler>();
serviceCollection.AddScoped<TestAnswerHandler>();
serviceCollection.AddScoped<PaginationExampleAnswerHandler>();

var serviceProvider = serviceCollection.BuildServiceProvider();
var memoryCache = new MemoryCache(new MemoryCacheOptions());

//await StartAsync(x =>
//{
//    x.Default<MainMenuAnswerHandler>();
//});


//temp implementation, imitation message handlers
for (int i = 0; i < 2; i++)
{
    var messageText = "Pagination";
    var scope = serviceProvider.CreateScope();
    var answerContext = new AnswerContext { UserId = 100, RegisterReplyKeyboardAsync = RegisterReplyKeyboardAsync };

    var (currentType, payload) = ResolveCurrentType(answerContext, messageText);
    await HandleType(scope, currentType!, payload, answerContext);

    scope.Dispose();
}


(Type, object) ResolveCurrentType(AnswerContext answerContext, string messageText)
{
    Type currentType = null;
    object payload = null;

    //check if user click on KeyboardActionButton
    if (memoryCache.TryGetValue($"{answerContext.UserId}_KEYBOARD", out Dictionary<string, RedirectionPayload> buttonsRedirections))
    {
        memoryCache.Remove($"{answerContext.UserId}_KEYBOARD");
        var resolvedAction = buttonsRedirections![messageText];
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
            currentType = typeof(MainMenuAnswerHandler);
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