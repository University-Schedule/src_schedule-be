using Schedule.TBot.Framework.AnswerResults;

namespace Schedule.TBot.Framework.Handlers
{
    public abstract class AnswerPayloadHandler<T> : BaseAnswerHandler where T : IPayload
    {
        public abstract Task<IAnswerResult> HandleAsync(MessageContext context, T query);
    }
}
