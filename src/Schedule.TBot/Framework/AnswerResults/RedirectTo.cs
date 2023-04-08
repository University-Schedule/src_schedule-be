namespace Schedule.TBot.Framework.AnswerResults
{
    public sealed class RedirectTo : IAnswerResult
    {
        private readonly Type _redirectType;
        private readonly object _payload;

        public RedirectTo(Type redirectType)
        {
            _redirectType = redirectType;
        }

        public RedirectTo(Type redirectType, object payload)
        {
            _redirectType = redirectType;
            _payload = payload;
        }

        public Type RedirectType => _redirectType;
        public object Payload => _payload;

        public Task ExecuteResultAsync() => Task.CompletedTask;
    }
}
