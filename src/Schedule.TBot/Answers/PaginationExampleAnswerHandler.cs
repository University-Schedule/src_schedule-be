using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;
using Schedule.TBot.Framework.Keyboard;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Answers
{
    public sealed class PaginationExampleAnswerHandler : AnswerQueryHandler<SimplePagedQuery>
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context, SimplePagedQuery query)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardActionButton("<", RedirectTo<PaginationExampleAnswerHandler, SimplePagedQuery>(new SimplePagedQuery() { Skip = query?.Skip - 1 ?? 0 })),
                        "Call me ☎️" ,
                        new KeyboardActionButton(">", RedirectTo<PaginationExampleAnswerHandler, SimplePagedQuery>(new SimplePagedQuery() { Skip = query?.Skip + 1 ?? 0 })),
                    }
                })
            {
                ResizeKeyboard = true
            };

            await AnswerAsync((query?.Skip ?? 0).ToString(), replyKeyboardMarkup);

            return Ok;
        }
    }

    public class SimplePagedQuery
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Count { get; set; }
    }
}
