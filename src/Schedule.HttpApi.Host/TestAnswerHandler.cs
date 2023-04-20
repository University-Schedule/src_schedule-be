using System.Threading.Tasks;
using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers
{
    public sealed class TestAnswerHandler : AnswerHandler
    {
        public override async Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            await AnswerAsync("Main menu!");
            return Ok;
        }
    }
}