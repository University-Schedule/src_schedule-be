using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Cards")]
public class CardModel : CreationAuditedEntity<string>
{
    public sealed override string Id { get; protected set; }
    public string LessonId { get; set; }
    public string Period { get; set; }
    public string Days { get; set; }
    public string Weeks { get; set; }
    public string Terms { get; set; }
    public string ClassRoomIds { get; set; }

    public CardModel()
    {
        
    }

    public CardModel(
        string id,
        string lessonId,
        string period,
        string days,
        string weeks,
        string terms,
        string classRoomIds)
    {
        Id = id;
        LessonId = lessonId;
        Period = period;
        Days = days;
        Weeks = weeks;
        Terms = terms;
        ClassRoomIds = classRoomIds;
    }
}