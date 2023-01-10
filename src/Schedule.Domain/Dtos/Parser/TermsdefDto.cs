using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Schedule.Dtos.Parser;

[SuppressMessage("ReSharper", "IdentifierTypo")]
public class TermsdefDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("terms")]
    public string Terms { get; set; }
}