using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Students")]
public class StudentModel : Entity<string>
{
    public string Id { get; }
    public string ClassId { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string PartnerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public StudentModel()
    {
        
    }

    public StudentModel(
        string id,
        string classId,
        string name,
        string number,
        string email,
        string mobile,
        string partnerId,
        string firstName,
        string lastName)
    {
        Id = id;
        ClassId = classId;
        Name = name;
        Number = number;
        Email = email;
        Mobile = mobile;
        PartnerId = partnerId;
        FirstName = firstName;
        LastName = lastName;
    }
}