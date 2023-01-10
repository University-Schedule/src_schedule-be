using Volo.Abp.Settings;

namespace Schedule.Settings;

public class ScheduleSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(new SettingDefinition(ScheduleSettings.Polesse.ApiUrl));
    }
}
