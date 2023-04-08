namespace Schedule.TBot.Framework
{
    public sealed class BotActionConfiguration
    {
        private Type _defaultHandler;
        private Dictionary<string, Type> _commandHandlers = new();
        private Dictionary<string, Type> _textHandlers = new();
        private string _token;

        public Type DefaultHandler => _defaultHandler;
        public string Token => _token;
        public Type GetCommandHandler(string command)
        {
            _commandHandlers.TryGetValue(command, out var handler);
            return handler;
        }
        public Type GetTextHandler(string command) 
        {
            _textHandlers.TryGetValue(command, out var handler);
            return handler;
        }

        public void Default<T>()
        {
            _defaultHandler = typeof(T);
        }

        public void Command<T>(params string[] commands)
        {
            foreach (var command in commands)
            {
                _commandHandlers.Add(command, typeof(T));
            }
        }

        public void Text<T>(params string[] texts)
        {
            foreach (var text in texts)
            {
                _textHandlers.Add(text, typeof(T));
            }
        }

        public void SetToken(string token)
        {
            _token = tiken;
        }
    }
}
