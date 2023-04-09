using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Answers
{
    public sealed class InputNameAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            await AnswerAsync("Please, input your name", new ReplyKeyboardRemove());

            return RedirectReceiving<AcceptNameAnswerHandler>(); ;
        }
    }
}
