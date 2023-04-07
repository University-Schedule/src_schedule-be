using Microsoft.Extensions.DependencyInjection;
using Schedule.TBot.Answers;

var serviceCollection = new ServiceCollection();
var serviceProvider = serviceCollection.BuildServiceProvider();


//await StartAsync(x =>
//{
//    x.Default<MainMenuAnswerHandler>();
//});


var scope = serviceProvider.CreateScope();
//scope.ServiceProvider.GetService<...>
scope.Dispose();