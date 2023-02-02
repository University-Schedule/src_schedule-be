using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "DaysDefs")]
public class DaysDefModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
    public string Name { get; set; }
    public string Short { get; set; }
    public string Days { get; set; }

    public DaysDefModel()
    {
        
    }

    public DaysDefModel(string id, string name, string @short, string days)
    {
        Id = id;
        Name = name;
        Short = @short;
        Days = days;
    }
}