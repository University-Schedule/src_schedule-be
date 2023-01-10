using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Schedule.Settings;

public interface IScheduleSettingManagementProvider : ISingletonDependency
{
    Task<string> GetPolesseApiUrlAsync();
}

public class ScheduleSettingManagementProvider : IScheduleSettingManagementProvider
{
    private readonly ISettingManagementStore _settingManagerStore;

    public ScheduleSettingManagementProvider(ISettingManagementStore settingManagerStore)
    {
        _settingManagerStore = settingManagerStore;
    }

    public async Task<string> GetPolesseApiUrlAsync()
    {
        return await _settingManagerStore.GetOrNullAsync(ScheduleSettings.Polesse.ApiUrl,
            ScheduleConsts.GlobalSettingValueProvider, null);
    }
}