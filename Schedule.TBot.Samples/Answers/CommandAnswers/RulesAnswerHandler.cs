using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers.CommandAnswers
{
    public sealed class RulesAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            await AnswerAsync("Rules: TODO");
            return RedirectTo<MenuAnswerHandler>();
        }
    }
}
