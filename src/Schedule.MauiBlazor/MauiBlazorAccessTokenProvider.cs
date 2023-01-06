using Schedule.MauiBlazor.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.IdentityModel.MauiBlazor;

namespace Schedule.MauiBlazor;

[ExposeServices(typeof(IAbpMauiAccessTokenProvider))]
public class MauiBlazorAccessTokenProvider : IAbpMauiAccessTokenProvider , ITransientDependency
{
    private readonly IScheduleApplicationSettingService _scheduleApplicationSettingService;

    public MauiBlazorAccessTokenProvider(IScheduleApplicationSettingService scheduleApplicationSettingService)
    {
        _scheduleApplicationSettingService = scheduleApplicationSettingService;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        return await _scheduleApplicationSettingService.GetAccessTokenAsync();
    }
}
