using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Framework.AnswerResults
{
    public sealed class RedirectTo : IAnswerResult
    {
        private readonly Type _redirectType;
        private readonly IPayload _payload;

        public RedirectTo(Type redirectType)
        {
            _redirectType = redirectType;
        }

        public RedirectTo(Type redirectType, IPayload payload)
        {
            _redirectType = redirectType;
            _payload = payload;
        }

        public Type RedirectType => _redirectType;
        public IPayload Payload => _payload;

        public Task ExecuteResultAsync() => Task.CompletedTask;
    }
}
