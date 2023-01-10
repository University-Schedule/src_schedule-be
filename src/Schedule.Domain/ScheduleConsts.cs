using Volo.Abp.Identity;

namespace Schedule;

public static class ScheduleConsts
{
    public const string DbTablePrefix = "App";
    public const string DbSchema = null;
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
    
    public const string DefaultValueSettingValueProvider = "D";
    public const string ConfigurationSettingValueProvider = "C";
    public const string GlobalSettingValueProvider = "G";
    public const string TenantSettingValueProvider = "T";
    public const string UserSettingValueProvider = "U";
}
