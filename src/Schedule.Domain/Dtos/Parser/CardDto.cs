using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class CardDto
{
    [DisplayName("lessonid")]
    public string LessonId { get; set; }
    
    [DisplayName("period")]
    public string Period { get; set; }
    
    [DisplayName("days")]
    public string Days { get; set; }
    
    [DisplayName("weeks")]
    public string Weeks { get; set; }
    
    [DisplayName("terms")]
    public string Terms { get; set; }
    
    [DisplayName("classroomids")]
    public string ClassRoomIds { get; set; }
}