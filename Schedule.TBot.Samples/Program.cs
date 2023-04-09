using Microsoft.Extensions.DependencyInjection;
using Schedule.TBot;
using Schedule.TBot.Answers;
using Schedule.TBot.Answers.CommandAnswers;
using Schedule.TBot.Answers.TextAnswers;
using Schedule.TBot.Extensions;

var serviceCollection = new ServiceCollection();
serviceCollection.AddMemoryCache();
serviceCollection.RegisterAnswerHandlers();

var serviceProvider = serviceCollection.BuildServiceProvider();

//http://t.me/net_framework_bot
await new TBot("6114166319:AAEokZNZlR3EuQ7SoYQKuJdGzzUUqf_Alno").RunAsync(x =>
{
    x.Default<MenuAnswerHandler>();
    x.Command<MenuAnswerHandler>("menu");
    x.Command<RulesAnswerHandler>("rules", "rule", "правила");
    x.Text<HelloExampleAnswerHandler>("hi", "hello");
}, serviceProvider);

Console.ReadLine();