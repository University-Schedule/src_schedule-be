using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Classes")]
public class ClassModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
    public string Name { get; set; }
    public string Short { get; set; }
    public string ClassRoomIds { get; set; }
    public string TeacherId { get; set; }
    public string Grade { get; set; }
    public string PartnerId { get; set; }

    public ClassModel()
    {
        
    }

    public ClassModel(
        string id,
        string name,
        string @short,
        string classRoomIds,
        string teacherId,
        string grade,
        string partnerId)
    {
        Id = id;
        Name = name;
        Short = @short;
        ClassRoomIds = classRoomIds;
        TeacherId = teacherId;
        Grade = grade;
        PartnerId = partnerId;
    }
}