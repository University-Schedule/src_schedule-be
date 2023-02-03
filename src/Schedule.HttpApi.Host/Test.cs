using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Schedule.Interfaces.Helper.Parser;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

// ReSharper disable IdentifierTypo

namespace Schedule;

public class Test : AsyncPeriodicBackgroundWorkerBase, ISingletonDependency
{
    public Test(AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory) 
        : base(timer, serviceScopeFactory)
    {
        Timer.Period = 300000; // 5 min
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Parser xml Palesse started...");
        
        var parseXmlHelper = workerContext.ServiceProvider.GetRequiredService<IParseXmlHelper>();

        await parseXmlHelper.ParsePolesseAsync();
        
        Logger.LogInformation("Parser xml Palesse finished!!!");
    }
}