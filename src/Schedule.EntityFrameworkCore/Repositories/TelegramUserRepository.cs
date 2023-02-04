using System;
using System.Linq;
using System.Threading.Tasks;
using Schedule.EntityFrameworkCore;
using Schedule.Enums;
using Schedule.Interfaces;
using Schedule.Models.Bot;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Schedule.Repositories;

public class TelegramUserRepository : EfCoreRepository<ScheduleDbContext, TelegramUser, Guid>, ITelegramUserRepository
{
    public TelegramUserRepository(IDbContextProvider<ScheduleDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async Task<TelegramUser> GetByTelegramIdAsync(long telegramUserId)
    {
        return (await GetDbSetAsync()).FirstOrDefault(x => x.TelegramId == telegramUserId);
    }

    public async Task UpdateUserStepAsync(long telegramUserId, EUserStep userStep)
    {
        var user = await GetByTelegramIdAsync(telegramUserId);

        user.CurrentStep = userStep.ToString();

        await UpdateAsync(user);
    }

    public async Task<EUserStep> GetUserStepAsync(long telegramUserId)
    {
        var user = await GetByTelegramIdAsync(telegramUserId);

        Enum.TryParse<EUserStep>(user.CurrentStep, out var step);

        return step;
    }

    public async Task SetGroupAsync(long telegramUserId, string group)
    {
        await SetData(telegramUserId, group);

    }

    public async Task SetTeacherAsync(long telegramUserId, string teacher)
    {
        await SetData(telegramUserId, teacher, true);
    }

    private async Task SetData(long telegramUserId, string scheduleGroup, bool isTeacher = false)
    {
        var user = await GetByTelegramIdAsync(telegramUserId);

        user.IsTeacher = isTeacher;
        user.ScheduleGroup = scheduleGroup;

        await UpdateAsync(user);
    }
}