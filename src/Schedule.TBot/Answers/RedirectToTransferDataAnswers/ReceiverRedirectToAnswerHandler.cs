using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers.TextAnswers
{
    public sealed class ReceiverRedirectToAnswerHandler : AnswerPayloadHandler<SamplePayload>
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context, SamplePayload payload)
        {
            await AnswerAsync($"Receiver: accepted {payload.Value}");
            return RedirectTo<MenuAnswerHandler>();
        }
    }
}
