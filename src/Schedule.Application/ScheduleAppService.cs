using Schedule.Localization;
using Volo.Abp.Application.Services;

namespace Schedule;

/* Inherit your application services from this class.
 */
public abstract class ScheduleAppService : ApplicationService
{
    protected ScheduleAppService()
    {
        LocalizationResource = typeof(ScheduleResource);
    }
}
