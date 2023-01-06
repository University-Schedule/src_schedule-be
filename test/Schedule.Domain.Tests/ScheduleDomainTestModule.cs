using Schedule.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Schedule;

[DependsOn(
    typeof(ScheduleEntityFrameworkCoreTestModule)
    )]
public class ScheduleDomainTestModule : AbpModule
{

}
