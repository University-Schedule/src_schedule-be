namespace Schedule.MauiBlazor.Settings;

public interface IScheduleApplicationSettingService
{   
   Task<string> GetAccessTokenAsync();
    
    Task SetAccessTokenAsync(string accessToken);
}