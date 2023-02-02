using System.Threading.Tasks;
using Schedule.Models.Parser;
using Volo.Abp.Domain.Repositories;

namespace Schedule.Interfaces;

public interface ISubjectRepository : IRepository<SubjectModel, string>
{
    Task DeleteAllRecordsAsync();
}