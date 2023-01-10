using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "StudentSubjects")]
public class StudentSubjectsModel : Entity<string>
{
    private readonly string _id = string.Empty;
    private const int MaxIdLength = 16;
    
    public string StudentId { get; set; }
    public string SubjectId { get; set; }
    public string SeminarGroup { get; set; }
    public string Importance { get; set; }
    public string AlternateFor { get; set; }

    public StudentSubjectsModel()
    {
        
    }

    public StudentSubjectsModel(
        string studentId,
        string subjectId,
        string seminarGroup,
        string importance,
        string alternateFor)
    {
        Id = _id.GetStringId(MaxIdLength);
        StudentId = studentId;
        SubjectId = subjectId;
        SeminarGroup = seminarGroup;
        Importance = importance;
        AlternateFor = alternateFor;
    }
}