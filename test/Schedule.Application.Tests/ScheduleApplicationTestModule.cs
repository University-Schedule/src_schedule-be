using Volo.Abp.Modularity;

namespace Schedule;

[DependsOn(
    typeof(ScheduleApplicationModule),
    typeof(ScheduleDomainTestModule)
    )]
public class ScheduleApplicationTestModule : AbpModule
{

}
