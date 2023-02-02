using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class LessonDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("subjectid")]
    public string SubjectId { get; set; }
    
    [DisplayName("classids")]
    public string ClassIds { get; set; }
    
    [DisplayName("groupids")]
    public string GroupIds { get; set; }
    
    [DisplayName("teacherids")]
    public string TeacherIds { get; set; }
    
    [DisplayName("classroomids")]
    public string ClassRoomIds { get; set; }
    
    [DisplayName("periodspercard")]
    public string PeriodsPerCard { get; set; }
    
    [DisplayName("periodsperweek")]
    public string PeriodsPerWeek { get; set; }
    
    [DisplayName("daysdefid")]
    public string DaysDefId { get; set; }
    
    [DisplayName("weeksdefid")]
    public string WeeksDefId { get; set; }
    
    [DisplayName("termsdefid")]
    public string TermsDefId { get; set; }
    
    [DisplayName("seminargroup")]
    public string SeminarGroup { get; set; }
    
    [DisplayName("capacity")]
    public string Capacity { get; set; }
    
    [DisplayName("partnerid")]
    public string PartnerId { get; set; }
}