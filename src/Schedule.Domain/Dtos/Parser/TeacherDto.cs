using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class TeacherDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("Gender")]
    public string Gender { get; set; }
    
    [DisplayName("color")]
    public string Color { get; set; }
    
    [DisplayName("email")]
    public string Email { get; set; }
    
    [DisplayName("mobile")]
    public string Mobile { get; set; }
    
    [DisplayName("partner_id")]
    public string PartnerId { get; set; }
    
    [DisplayName("firstname")]
    public string FirstName { get; set; }
    
    [DisplayName("lastname")]
    public string LastName { get; set; }
}