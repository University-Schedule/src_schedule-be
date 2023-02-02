using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "StudentSubjects")]
public class StudentSubjectsModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
    public string StudentId { get; set; }
    public string SubjectId { get; set; }
    public string SeminarGroup { get; set; }
    public string Importance { get; set; }
    public string AlternateFor { get; set; }

    public StudentSubjectsModel()
    {
        
    }

    public StudentSubjectsModel(
        string id,
        string studentId,
        string subjectId,
        string seminarGroup,
        string importance,
        string alternateFor)
    {
        Id = id;
        StudentId = studentId;
        SubjectId = subjectId;
        SeminarGroup = seminarGroup;
        Importance = importance;
        AlternateFor = alternateFor;
    }
}