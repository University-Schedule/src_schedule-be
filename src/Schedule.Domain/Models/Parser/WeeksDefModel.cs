using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "WeeksDefs")]
public class WeeksDefModel : Entity<string>
{
    public string Name { get; set; }
    public string Short { get; set; }
    public string Weeks { get; set; }

    public WeeksDefModel()
    {
        
    }

    public WeeksDefModel(string id, string name, string @short, string weeks)
    {
        Id = id;
        Name = name;
        Short = @short;
        Weeks = weeks;
    }
}