using System.Threading.Tasks;

namespace Schedule.Data;

public interface IScheduleDbSchemaMigrator
{
    Task MigrateAsync();
}
