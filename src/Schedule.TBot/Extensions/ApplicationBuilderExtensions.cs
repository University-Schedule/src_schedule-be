using Microsoft.AspNetCore.Builder;
using Schedule.TBot.Framework;

namespace Schedule.TBot.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static Task RunTBotAsync(this IApplicationBuilder applicationBuilder, string token, Action<BotActionConfiguration> configure, CancellationToken cancellationToken = default)
            => new TBot(token).RunAsync(configure, applicationBuilder.ApplicationServices, cancellationToken);
    }
}
