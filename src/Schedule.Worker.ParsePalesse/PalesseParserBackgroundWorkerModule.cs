/*using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;
// ReSharper disable IdentifierTypo

namespace Schedule.Worker.ParsePalesse;

[DependsOn(typeof(AbpBackgroundWorkersModule))]
public class PalesseParserBackgroundWorkerModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<PalesseParserBackgroundWorker>();
    }
}*/