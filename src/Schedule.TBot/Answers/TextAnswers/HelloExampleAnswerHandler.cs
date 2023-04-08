using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers.TextAnswers
{
    public sealed class HelloExampleAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            await AnswerAsync("Hello my friend!");
            return Ok;
        }
    }
}
