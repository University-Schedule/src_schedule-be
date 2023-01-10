using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class StudentSubjectsDto
{
    [DisplayName("studentid")]
    public string StudentId { get; set; }
    
    [DisplayName("subjectid")]
    public string SubjectId { get; set; }
    
    [DisplayName("seminargroup")]
    public string SeminarGroup { get; set; }
    
    [DisplayName("importance")]
    public string Importance { get; set; }
    
    [DisplayName("alternatefor")]
    public string AlternateFor { get; set; }
}