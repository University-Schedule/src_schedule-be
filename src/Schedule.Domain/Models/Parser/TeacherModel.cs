using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Teachers")]
public class TeacherModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
    public string Name { get; set; }
    public string Short { get; set; }
    public string Gender { get; set; }
    public string Color { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string PartnerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public TeacherModel()
    {
        
    }

    public TeacherModel(
        string id, 
        string name, 
        string @short,
        string gender,
        string color,
        string email,
        string mobile,
        string partnerId,
        string firstName,
        string lastName)
    {
        Id = id;
        Name = name;
        Short = @short;
        Gender = gender;
        Color = color;
        Email = email;
        Mobile = mobile;
        PartnerId = partnerId;
        FirstName = firstName;
        LastName = lastName;
    }
}