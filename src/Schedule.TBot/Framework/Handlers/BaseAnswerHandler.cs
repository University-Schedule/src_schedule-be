using Schedule.TBot.Framework.AnswerResults;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Framework.Handlers
{
    public abstract class BaseAnswerHandler
    {
        public AnswerContext AnswerContext { get; set; }

        protected IAnswerResult Ok => new OkResult();

        protected IAnswerResult RedirectTo<T>() where T : BaseAnswerHandler
        {
            return new RedirectTo(typeof(T));
        }

        protected IAnswerResult RedirectTo<T, TPayload>(TPayload payload) 
            where T : AnswerPayloadHandler<TPayload> 
            where TPayload : IPayload
        {
            return new RedirectTo(typeof(T), payload);
        }

        protected IAnswerResult RedirectReceiving<T>() where T : AnswerHandler
        {
            return new RedirectReceiving(typeof(T));
        }

        protected IAnswerResult RedirectReceiving<T, TPayload>(TPayload payload)
         where T : AnswerPayloadHandler<TPayload>
         where TPayload : IPayload
        {
            return new RedirectReceiving(typeof(T), payload);
        }

        protected async Task AnswerAsync(string text, ReplyMarkupBase keyboard = null)
        {
            //handle actions keyboards
            if (keyboard is ReplyKeyboardMarkup replyKeyboard && replyKeyboard is not null)
            {
                await AnswerContext.RegisterReplyKeyboardAsync(AnswerContext.UserId, replyKeyboard);
            }
            await AnswerContext.BotClient.SendTextMessageAsync(AnswerContext.UserId, text, replyMarkup: keyboard);
        }

        
    }
}
