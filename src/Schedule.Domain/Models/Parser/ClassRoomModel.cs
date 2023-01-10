using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "ClassRooms")]
public class ClassRoomModel : Entity<string>
{
    public string Name { get; set; }
    public string Short { get; set; }
    public string Capacity { get; set; }
    public string BuildingId { get; set; }
    public string PartnerId { get; set; }

    public ClassRoomModel()
    {
        
    }

    public ClassRoomModel(
        string id,
        string name,
        string @short,
        string capacity,
        string buildingId,
        string partnerId)
    {
        Name = name;
        Short = @short;
        Capacity = capacity;
        BuildingId = buildingId;
        PartnerId = partnerId;
        Id = id;
    }
}