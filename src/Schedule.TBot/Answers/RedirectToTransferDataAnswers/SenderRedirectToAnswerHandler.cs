using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers.TextAnswers
{
    public sealed class SenderRedirectToAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            var payload = new SamplePayload { Value = 3 };
            await AnswerAsync($"Sender: Send {payload.Value}");
            return RedirectTo<ReceiverRedirectToAnswerHandler, SamplePayload>(payload);
        }
    }

    public class SamplePayload: IPayload 
    {
        public int Value { get; set; }
    }
}
