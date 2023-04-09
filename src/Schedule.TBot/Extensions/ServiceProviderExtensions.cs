using Microsoft.Extensions.DependencyInjection;

namespace Schedule.TBot.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static IServiceScope CreateScope(this IServiceProvider provider)
        {
            return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        }
    }
}
