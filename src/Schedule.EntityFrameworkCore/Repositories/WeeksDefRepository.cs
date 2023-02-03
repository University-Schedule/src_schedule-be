using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Schedule.EntityFrameworkCore;
using Schedule.Interfaces;
using Schedule.Models.Parser;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Schedule.Repositories;

public class WeeksDefRepository : EfCoreRepository<ScheduleDbContext, WeeksDefModel, string>, IWeeksDefRepository
{
    public WeeksDefRepository(IDbContextProvider<ScheduleDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async Task DeleteAllRecordsAsync()
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.ExecuteSqlRawAsync("DELETE AppWeeksDefs");
    }
}