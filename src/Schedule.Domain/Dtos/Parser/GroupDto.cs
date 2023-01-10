using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class GroupDto
{
    [DisplayName("id")]
    public string Id { get; }
    
    [DisplayName("classid")]
    public string ClassId { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("entireclass")]
    public string EntireClass { get; set; }
    
    [DisplayName("divisiontag")]
    public string DivisionTag { get; set; }
    
    [DisplayName("studentcount")]
    public string StudentCount { get; set; }
    
    [DisplayName("studentids")]
    public string StudentIds { get; set; }
}