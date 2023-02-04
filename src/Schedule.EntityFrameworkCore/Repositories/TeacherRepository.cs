using System.Collections.Generic;
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

public class TeacherRepository : EfCoreRepository<ScheduleDbContext, TeacherModel, string>, ITeacherRepository
{
    public TeacherRepository(IDbContextProvider<ScheduleDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteAllRecordsAsync()
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.ExecuteSqlRawAsync("DELETE AppTeachers");
    }

    public async Task<List<string>> GetListTeachersByLetterAsync(string letter)
    {
        return (await GetDbSetAsync())
            .Where(x => x.Short.StartsWith(letter) && !x.Short.Contains(BotConst.Cathedra))
            .Select(s => s.Short)
            .ToList();
    }
    
    public async Task<List<string>> GetListFirstLettersTeachersAsync()
    {
        return (await GetDbSetAsync())
            .Where(x => !x.Short.Contains(BotConst.Cathedra))
            .Select(s => s.Short.Substring(0,1))
            .Distinct()
            .ToList();
    }

    public async Task<bool> TeacherExistAsync(string teacher)
    {
        var userGroup = (await GetDbSetAsync())
            .FirstOrDefault(x => x.Short.ToLower() == teacher.ToLower());

        return userGroup != null;
    }
}