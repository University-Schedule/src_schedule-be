using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Lessons")]
public class LessonModel : Entity<string>
{
    public string Id { get; }
    public string SubjectId { get; set; }
    public string ClassIds { get; set; }
    public string GroupIds { get; set; }
    public string TeacherIds { get; set; }
    public string ClassRoomIds { get; set; }
    public string PeriodsPerCard { get; set; }
    public string PeriodsPerWeek { get; set; }
    public string DaysDefId { get; set; }
    public string WeeksDefId { get; set; }
    public string TermsDefId { get; set; }
    public string SeminarGroup { get; set; }
    public string Capacity { get; set; }
    public string PartnerId { get; set; }

    public LessonModel()
    {
        
    }

    public LessonModel(
        string id,
        string subjectId,
        string classIds,
        string groupIds,
        string teacherIds,
        string classRoomIds,
        string periodsPerCard,
        string periodsPerWeek,
        string daysDefId,
        string weeksDefId,
        string termsDefId,
        string seminarGroup,
        string capacity,
        string partnerId)
    {
        Id = id;
        SubjectId = subjectId;
        ClassIds = classIds;
        GroupIds = groupIds;
        TeacherIds = teacherIds;
        ClassRoomIds = classRoomIds;
        PeriodsPerCard = periodsPerCard;
        PeriodsPerWeek = periodsPerWeek;
        DaysDefId = daysDefId;
        WeeksDefId = weeksDefId;
        TermsDefId = termsDefId;
        SeminarGroup = seminarGroup;
        Capacity = capacity;
        PartnerId = partnerId;
    }
}