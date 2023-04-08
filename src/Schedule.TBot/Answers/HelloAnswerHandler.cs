using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Answers
{
    public sealed class HelloAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            await AnswerAsync("Hello my friend!!!");

            return Ok;
        }
    }
}
