using System.Threading.Tasks;
using Schedule.Models.Parser;
using Volo.Abp.Domain.Repositories;

namespace Schedule.Interfaces;

public interface IBuildingRepository : IRepository<BuildingModel, string>
{
    Task DeleteAllRecordsAsync();
}