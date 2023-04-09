using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;
using Schedule.TBot.Framework.Keyboard;
using Schedule.TBot.Framework;
using Schedule.TBot.Utils;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Schedule.TBot
{
    public sealed class MessageHandler
    {
        private const string KEYBOARD_CACHE_KEY = "{0}_KEYBOARD";

        private readonly IMemoryCache _memoryCache;
        private readonly ITelegramBotClient _botClient;
        private readonly BotActionConfiguration _actionConfiguration;
        private readonly IServiceScope _serviceScope;

        public MessageHandler(
                ITelegramBotClient botClient,
                BotActionConfiguration actionConfiguration,
                IServiceScope serviceScope
                )
        {
            _botClient = botClient;
            _actionConfiguration = actionConfiguration;
            _serviceScope = serviceScope;
            _memoryCache = serviceScope.ServiceProvider.GetService<IMemoryCache>()!;
        }

        public async Task HandleAsync(Message message)
        {
            var answerContext = new AnswerContext
            {
                UserId = message.From!.Id,
                RegisterReplyKeyboardAsync = RegisterReplyKeyboardAsync,
                BotClient = _botClient
            };

            var redirectionContext = ResolveRiderectionContext(answerContext, message.Text!);
            await HandleTypeAsync(redirectionContext, answerContext, message);
        }

        private RedirectionContext ResolveRiderectionContext(AnswerContext answerContext, string messageText)
        {
            var resolvers = new Func<RedirectionContext?>[] {
                () => GetKeyboardRedirectionContext(answerContext, messageText),
                () => GetRedirectReceivingContext(answerContext),
                () => GetConfigurationRedirectionContext(messageText)
            };

            foreach (var resolver in resolvers)
            {
                var context = resolver();
                if (context != null) return context;
            }
            return null!;
        }

        private RedirectionContext? GetKeyboardRedirectionContext(AnswerContext answerContext, string messageText)
        {
            var keyboardKey = string.Format(KEYBOARD_CACHE_KEY, answerContext.UserId);
            if (_memoryCache.TryGetValue(keyboardKey, out Dictionary<string, RedirectionContext>? buttonsRedirections))
            {
                _memoryCache.Remove(keyboardKey);
                buttonsRedirections!.TryGetValue(messageText, out var resolvedAction);
                return resolvedAction;
            }
            return null;
        }
        private RedirectionContext? GetRedirectReceivingContext(AnswerContext answerContext)
        {
            if (_memoryCache.TryGetValue(answerContext.UserId, out RedirectionContext? resolvedType))
            {
                _memoryCache.Remove(answerContext.UserId);
                return resolvedType;
            }
            return null;
        }
        private RedirectionContext GetConfigurationRedirectionContext(string messageText)
        {
            if (messageText.StartsWith('/') && _actionConfiguration.GetCommandHandler(messageText) is not null)
            {
                return new RedirectionContext(_actionConfiguration.GetCommandHandler(messageText));
            }
            if (_actionConfiguration.GetTextHandler(messageText) is not null)
            {
                return new RedirectionContext(_actionConfiguration.GetTextHandler(messageText));
            }
            return new RedirectionContext(_actionConfiguration.DefaultHandler);
        }

        async Task HandleTypeAsync(RedirectionContext redirectionContext, AnswerContext context, Message message)
        {
            if (_serviceScope.ServiceProvider.GetService(redirectionContext.Type) is BaseAnswerHandler currentHandler)
            {
                currentHandler.AnswerContext = context;
                var messageContext = new MessageContext() { Text = message.Text, UserId = context.UserId };

                IAnswerResult? result = currentHandler switch
                {
                    AnswerHandler handler => await handler.HandleAsync(messageContext),
                    _ when TypeUtils.IsSubclassOfRawGeneric(typeof(AnswerPayloadHandler<>), redirectionContext.Type) =>
                        await (Task<IAnswerResult?>)currentHandler.GetType().GetMethod(nameof(AnswerPayloadHandler<IPayload>.HandleAsync)).Invoke(currentHandler, new object[] { messageContext, redirectionContext.Payload }),
                    _ => new OkResult()
                };

                //handle result
                if (result is RedirectTo redirectTo)
                {
                    await HandleTypeAsync(new RedirectionContext(redirectTo.RedirectType, redirectTo.Payload), context, message);
                }
                else if (result is RedirectReceiving redirectReceiving)
                {
                    _memoryCache.Set(context.UserId, new RedirectionContext(redirectReceiving.RedirectType, redirectReceiving.Payload));
                }
            }
        }

        Task RegisterReplyKeyboardAsync(long userId, ReplyMarkupBase replyMarkup)
        {
            var keyboardCacheKey = string.Format(KEYBOARD_CACHE_KEY, userId);
            if (replyMarkup is ReplyKeyboardMarkup replyKeyboard)
            {
                var buttonsRedirections = new Dictionary<string, RedirectionContext>();
                foreach (var row in replyKeyboard.Keyboard)
                {
                    foreach (var button in row)
                    {
                        if (button is KeyboardActionButton actionButton && actionButton.Action is RedirectTo redirectTo)
                        {
                            buttonsRedirections.Add(button.Text, new RedirectionContext(redirectTo.RedirectType, redirectTo.Payload));
                        }
                    }
                }
                _memoryCache.Set(keyboardCacheKey, buttonsRedirections);
            }
            else if (replyMarkup is ReplyKeyboardRemove replyKeyboardRemove)
            {
                _memoryCache.Remove(keyboardCacheKey);
            }
            return Task.CompletedTask;
        }
    }
}
