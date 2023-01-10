using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "DaysDefs")]
public class DaysDefModel : Entity<string>
{
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