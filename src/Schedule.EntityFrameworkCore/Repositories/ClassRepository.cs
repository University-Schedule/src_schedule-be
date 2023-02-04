using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Schedule.Constants;
using Schedule.EntityFrameworkCore;
using Schedule.Interfaces;
using Schedule.Models.Parser;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Schedule.Repositories;

public class ClassRepository : EfCoreRepository<ScheduleDbContext, ClassModel, string>, IClassRepository
{
    public ClassRepository(IDbContextProvider<ScheduleDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task DeleteAllRecordsAsync()
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.ExecuteSqlRawAsync("DELETE AppClasses");
    }

    public async Task<bool> GroupExistAsync(string group)
    {
        var userGroup = (await GetDbSetAsync())
            .FirstOrDefault(x => x.Name.ToLower() == group.ToLower());

        return userGroup != null;
    }
}