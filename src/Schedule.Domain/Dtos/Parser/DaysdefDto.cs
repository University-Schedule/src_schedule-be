using System.ComponentModel;
using Volo.Abp.Domain.Entities;

namespace Schedule.Dtos.Parser;

public class DaysdefDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("days")]
    public string Days { get; set; }
}