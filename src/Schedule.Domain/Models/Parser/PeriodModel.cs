using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Periods")]
public class PeriodModel : Entity<string>
{
    private readonly string _id = string.Empty;
    private const int MaxIdLength = 16;
    
    public string Period { get; set; }
    public string Short { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public PeriodModel()
    {
        
    }

    public PeriodModel(
        string period,
        string @short,
        string startTime,
        string endTime)
    {
        Id = _id.GetStringId(MaxIdLength);
        Period = period;
        Short = @short;
        StartTime = startTime;
        EndTime = endTime;
    }
}