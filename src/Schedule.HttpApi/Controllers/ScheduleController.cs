using Schedule.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Schedule.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ScheduleController : AbpControllerBase
{
    protected ScheduleController()
    {
        LocalizationResource = typeof(ScheduleResource);
    }
}
