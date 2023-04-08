using Microsoft.Extensions.DependencyInjection;
using Schedule.TBot.Framework.Handlers;

namespace Schedule.TBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAnswerHandlers(this ServiceCollection serviceDescriptors)
        {
            var answerHandlers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => !x.IsAbstract && typeof(BaseAnswerHandler).IsAssignableFrom(x)).ToArray();
            foreach (var handler in answerHandlers)
            {
                serviceDescriptors.AddScoped(handler);
            }
        }

    }
}
