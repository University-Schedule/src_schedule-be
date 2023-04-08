using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers.TextAnswers
{
    public sealed class ReceiverRedirectReceivingAnswerHandler : AnswerPayloadHandler<SampleRedirectReceivingPayload>
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context, SampleRedirectReceivingPayload payload)
        {
            await AnswerAsync($"Receiver text: {context.Text}");
            await AnswerAsync($"Receiver: '{payload.Value}' value accepted from sender");

            return RedirectTo<MenuAnswerHandler>();
        }
    }
}
