using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "TermsDefs")]
public class TermsDefModel : Entity<string>
{
    public string Name { get; set; }
    public string Short { get; set; }
    public string Terms { get; set; }

    public TermsDefModel()
    {
        
    }

    public TermsDefModel(string id, string name, string @short, string terms)
    {
        Id = id;
        Name = name;
        Short = @short;
        Terms = terms;
    }
}