using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Framework.AnswerResults
{
    public sealed class RedirectReceiving : IAnswerResult
    {
        private readonly Type _redirectType;
        private readonly IPayload _payload;

        public RedirectReceiving(Type redirectType)
        {
            _redirectType = redirectType;
        }

        public RedirectReceiving(Type redirectType, IPayload payload)
        {
            _redirectType = redirectType;
            _payload = payload;
        }

        public Type RedirectType => _redirectType;
        public IPayload Payload => _payload;

        public Task ExecuteResultAsync() => Task.CompletedTask;
    }
}
