using System;
using System.Threading.Tasks;
using Schedule.Enums;
using Schedule.Models.Bot;
using Volo.Abp.Domain.Repositories;

namespace Schedule.Interfaces;

public interface ITelegramUserRepository : IRepository<TelegramUser, Guid>
{
    Task<TelegramUser> GetByTelegramIdAsync(long telegramUserId);

    Task UpdateUserStepAsync(long telegramUserId, EUserStep userStep);

    Task<EUserStep> GetUserStepAsync(long telegramUserId);

    Task SetGroupAsync(long telegramUserId, string group);

    Task SetTeacherAsync(long telegramUserId, string teacher);
}