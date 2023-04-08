using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Framework
{
    public sealed class RedirectionPayload
    {
        public Type Type { get; set; }
        public IPayload Payload { get; set; }
    }
}
