using System.Threading.Tasks;
using Schedule.Models.Parser;
using Volo.Abp.Domain.Repositories;

namespace Schedule.Interfaces;

public interface IClassRepository : IRepository<ClassModel, string>
{
    Task DeleteAllRecordsAsync();

    Task<bool> GroupExistAsync(string group);
}