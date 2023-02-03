using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Periods")]
public class PeriodModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
    public string Period { get; set; }
    public string Short { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public PeriodModel()
    {
        
    }

    public PeriodModel(
        string id,
        string period,
        string @short,
        string startTime,
        string endTime)
    {
        Id = id;
        Period = period;
        Short = @short;
        StartTime = startTime;
        EndTime = endTime;
    }
}