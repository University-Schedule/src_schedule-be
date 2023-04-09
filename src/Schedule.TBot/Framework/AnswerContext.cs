using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Schedule.TBot.Framework
{
    public sealed class AnswerContext
    {
        public ITelegramBotClient BotClient { get; init; }
        public long UserId { get; init; }
        public Func<long, ReplyMarkupBase, Task> RegisterReplyKeyboardAsync { get; init; }
    }
}
