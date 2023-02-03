using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "TermsDefs")]
public class TermsDefModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
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