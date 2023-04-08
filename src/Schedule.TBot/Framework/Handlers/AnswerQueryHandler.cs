using Schedule.TBot.Framework.AnswerResults;

namespace Schedule.TBot.Framework.Handlers
{
    public abstract class AnswerQueryHandler<T> : BaseAnswerHandler
    {
        public abstract Task<IAnswerResult> HandleAsync(MessageContext context, T query);
    }
}
