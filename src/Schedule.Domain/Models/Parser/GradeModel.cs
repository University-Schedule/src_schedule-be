using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Grades")]
public class GradeModel : Entity<string>
{
    private readonly string _id = string.Empty;
    private const int MaxIdLength = 16;
    
    public string Grade { get; set; }
    public string Name { get; set; }
    public string Short { get; set; }

    public GradeModel()
    {
        
    }

    public GradeModel(string grade, string name, string @short)
    {
        Grade = grade;
        Name = name;
        Short = @short;
        Id = _id.GetStringId(MaxIdLength);
    }
}