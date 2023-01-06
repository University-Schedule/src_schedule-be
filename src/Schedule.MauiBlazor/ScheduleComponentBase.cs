using Schedule.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Schedule.MauiBlazor;

public abstract class ScheduleComponentBase : AbpComponentBase
{
    protected ScheduleComponentBase()
    {
        LocalizationResource = typeof(ScheduleResource);
    }
}