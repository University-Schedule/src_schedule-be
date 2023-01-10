using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class SubjectDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("partner_id")]
    public string PartnerId { get; set; }
}