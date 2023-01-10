using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Buildings")]
public class BuildingModel : Entity<string>
{
    public string Name { get; set; }
    public string PartnerId { get; set; }

    public BuildingModel()
    {
        
    }

    public BuildingModel(string id, string name, string partnerId)
    {
        Id = id;
        Name = name;
        PartnerId = partnerId;
    }
}