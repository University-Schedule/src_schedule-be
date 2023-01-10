using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Schedule.Settings;

public class ScheduleSettingManagementStore: SettingManagementProvider, ITransientDependency
{
    public override string Name => "Custom";
    
    public ScheduleSettingManagementStore(ISettingManagementStore store) 
        : base(store)
    {
        store.SetAsync(ScheduleSettings.Polesse.ApiUrl,
            "https://www.polessu.by/ruz/term2/tt.xml", ScheduleConsts.GlobalSettingValueProvider, null);
    }
}