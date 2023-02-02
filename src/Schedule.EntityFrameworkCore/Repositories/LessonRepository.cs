using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Schedule.EntityFrameworkCore;
using Schedule.Interfaces;
using Schedule.Models.Parser;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Schedule.Repositories;

public class LessonRepository : EfCoreRepository<ScheduleDbContext, LessonModel, string>, ILessonRepository
{
    public LessonRepository(IDbContextProvider<ScheduleDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteAllRecordsAsync()
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.ExecuteSqlRawAsync("DELETE AppLessons");
    }
}