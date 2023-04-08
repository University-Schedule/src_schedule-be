namespace Schedule.TBot.Framework.AnswerResults
{
    public sealed class RedirectReceiving : IAnswerResult
    {
        private readonly Type _redirectType;

        public RedirectReceiving(Type redirectType)
        {
            _redirectType = redirectType;
        }

        public Type RedirectType => _redirectType;

        public Task ExecuteResultAsync() => Task.CompletedTask;
    }
}
