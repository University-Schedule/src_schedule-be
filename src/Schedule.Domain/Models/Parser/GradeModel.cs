using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Grades")]
public class GradeModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
    public string Grade { get; set; }
    public string Name { get; set; }
    public string Short { get; set; }

    public GradeModel()
    {
        
    }

    public GradeModel(string id, string grade, string name, string @short)
    {
        Id = id;
        Grade = grade;
        Name = name;
        Short = @short;
    }
}