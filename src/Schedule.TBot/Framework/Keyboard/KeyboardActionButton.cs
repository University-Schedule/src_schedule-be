using Schedule.TBot.Framework.AnswerResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Framework.Keyboard
{
    public sealed class KeyboardActionButton: KeyboardButton
    {
        public IAnswerResult Action { get; init; }

        public KeyboardActionButton(string text, IAnswerResult action) : base(text) 
        {
            Action = action;
        }
    }
}
