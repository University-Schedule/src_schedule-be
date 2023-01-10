using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class GradeDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("grade")]
    public string Grade { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
}