using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class StudentsDto
{
    [DisplayName("id")]
    public string Id { get; }
    
    [DisplayName("classid")]
    public string ClassId { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("number")]
    public string Number { get; set; }
    
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