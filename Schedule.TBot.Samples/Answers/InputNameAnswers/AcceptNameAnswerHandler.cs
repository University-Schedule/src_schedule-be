using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Answers
{
    public sealed class AcceptNameAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            if(context.Text.ToLower() == "cancel")
            {
                await AnswerAsync("Input canceled.");
                return RedirectTo<MenuAnswerHandler>(); ;
            }

            if (context.Text.Length < 5)
            {
                await AnswerAsync("Please, input more than 5 letters... (you can cancel input, send 'cancel')");
                return RedirectReceiving<AcceptNameAnswerHandler>(); ;
            }

            await AnswerAsync($"{context.Text} name applied.");
            return RedirectTo<MenuAnswerHandler>(); ;
        }
    }
}
