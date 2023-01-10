using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Subjects")]
public class SubjectModel : Entity<string>
{
    public string Name { get; set; }
    public string Short { get; set; }
    public string PartnerId { get; set; }

    public SubjectModel()
    {
        
    }

    public SubjectModel(string id, string name, string @short, string partnerId)
    {
        Id = id;
        Name = name;
        Short = @short;
        PartnerId = partnerId;
    }
}