using Schedule.TBot.Answers.CommandAnswers;
using Schedule.TBot.Answers.TextAnswers;
using Schedule.TBot.Framework;
using Schedule.TBot.Framework.AnswerResults;
using Schedule.TBot.Framework.Handlers;
using Schedule.TBot.Framework.Keyboard;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Answers
{
    public sealed class MenuAnswerHandler : AnswerHandler
    {
        public async override Task<IAnswerResult> HandleAsync(MessageContext context)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardActionButton("Help me", RedirectTo<RulesAnswerHandler>()),
                        new KeyboardActionButton("Pagination", RedirectTo<PaginationExampleAnswerHandler, SimplePagedPayload>(new SimplePagedPayload() { Skip = 3 }))
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardActionButton("Input name", RedirectTo<InputNameAnswerHandler>())
                    },
                    new KeyboardButton[]
                    {
                        new KeyboardActionButton("RedirectTo transfer", RedirectTo<SenderRedirectToAnswerHandler>()),
                        new KeyboardActionButton("RedirectReceiving transfer", RedirectTo<SenderRedirectReceivingAnswerHandler>())
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
