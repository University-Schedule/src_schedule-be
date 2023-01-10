using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class PeriodDto
{
    [DisplayName("period")]
    public string Period { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("starttime")]
    public string StartTime { get; set; }
    
    [DisplayName("endtime")]
    public string EndTime { get; set; }
}