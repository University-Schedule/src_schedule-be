using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;
using Schedule.TBot.Framework.Keyboard;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Answers
{
    public sealed class MainMenuAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardActionButton("Help me", RedirectTo<RulesAnswerHandler>()),
                        new KeyboardActionButton("Pagination", RedirectTo<PaginationExampleAnswerHandler, SimplePagedQuery>(new SimplePagedQuery() { Skip = 5 })),
                        "Call me ☎️" 
                    }
                })
            {
                ResizeKeyboard = true
            };

            await AnswerAsync("Main menu!", replyKeyboardMarkup);

            return Ok;
        }
    }
}
