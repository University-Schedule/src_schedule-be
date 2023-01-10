using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Cards")]
public class CardModel : Entity<string>
{
    private readonly string _id = string.Empty;
    private const int MaxIdLength = 16;
    
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
        string lessonId,
        string period,
        string days,
        string weeks,
        string terms,
        string classRoomIds)
    {
        LessonId = lessonId;
        Period = period;
        Days = days;
        Weeks = weeks;
        Terms = terms;
        ClassRoomIds = classRoomIds;
        Id = _id.GetStringId(MaxIdLength);
    }
}