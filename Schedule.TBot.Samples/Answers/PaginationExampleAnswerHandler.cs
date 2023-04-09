using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;
using Schedule.TBot.Framework.Keyboard;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Answers
{
    public sealed class PaginationExampleAnswerHandler : AnswerPayloadHandler<SimplePagedPayload>
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context, SimplePagedPayload query)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardActionButton("<", RedirectTo<PaginationExampleAnswerHandler, SimplePagedPayload>(new SimplePagedPayload() { Skip = query?.Skip - 1 ?? 0 })),
                        "Just Text ☎️" ,
                        new KeyboardActionButton(">", RedirectTo<PaginationExampleAnswerHandler, SimplePagedPayload>(new SimplePagedPayload() { Skip = query?.Skip + 1 ?? 0 })),
                    }
                })
            {
                ResizeKeyboard = true
            };

            await AnswerAsync((query?.Skip ?? 0).ToString(), replyKeyboardMarkup);

            return Ok;
        }
    }

    public class SimplePagedPayload : IPayload
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Count { get; set; }
    }
}
