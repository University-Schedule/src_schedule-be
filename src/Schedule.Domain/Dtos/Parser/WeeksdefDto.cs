using System.ComponentModel;

namespace Schedule.Dtos.Parser;

// ReSharper disable once IdentifierTypo
public class WeeksdefDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("weeks")]
    public string Weeks { get; set; }
}