using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class ClassDto
{
    [DisplayName("id")]
    public string Id { get; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("classroomids")]
    public string ClassRoomIds { get; set; }
    
    [DisplayName("teacherid")]
    public string TeacherId { get; set; }
    
    [DisplayName("grade")]
    public string Grade { get; set; }
    
    [DisplayName("partner_id")]
    public string PartnerId { get; set; }
}