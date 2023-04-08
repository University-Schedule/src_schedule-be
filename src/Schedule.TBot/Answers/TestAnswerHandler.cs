using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers
{
    public sealed class TestAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            await AnswerAsync("In test...");

            return RedirectTo<MainMenuAnswerHandler>(); ;
        }
    }
}
