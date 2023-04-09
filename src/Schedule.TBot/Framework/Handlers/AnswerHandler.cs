using Schedule.TBot.Framework.AnswerResults;
namespace Schedule.TBot.Framework.Handlers
{
    public abstract class AnswerHandler: BaseAnswerHandler
    {
        public abstract Task<IAnswerResult> HandleAsync(MessageContext context);
    }
}
