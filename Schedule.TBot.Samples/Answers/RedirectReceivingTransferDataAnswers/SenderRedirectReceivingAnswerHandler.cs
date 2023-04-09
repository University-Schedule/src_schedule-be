using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers.TextAnswers
{
    public sealed class SenderRedirectReceivingAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            var payload = new SampleRedirectReceivingPayload { Value = 3 };
            await AnswerAsync($"Sender: Send {payload.Value}");
            await AnswerAsync("Please enter any text");

            return RedirectReceiving<ReceiverRedirectReceivingAnswerHandler, SampleRedirectReceivingPayload>(payload);
        }
    }

    public class SampleRedirectReceivingPayload : IPayload 
    {
        public int Value { get; set; }
    }
}
