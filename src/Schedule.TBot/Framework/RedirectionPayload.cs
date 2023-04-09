using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Framework
{
    public sealed class RedirectionContext
    {
        public Type Type { get; init; }
        public IPayload Payload { get; init; }

        public RedirectionContext(Type type)
        {
            Type = type;
        }

        public RedirectionContext(Type type, IPayload payload)
        {
            Type = type;
            Payload = payload;
        }
    }
}
