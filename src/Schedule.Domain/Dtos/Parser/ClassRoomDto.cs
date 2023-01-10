using System.ComponentModel;

namespace Schedule.Dtos.Parser;

public class ClassRoomDto
{
    [DisplayName("id")]
    public string Id { get; set; }
    
    [DisplayName("name")]
    public string Name { get; set; }
    
    [DisplayName("short")]
    public string Short { get; set; }
    
    [DisplayName("capacity")]
    public string Capacity { get; set; }
    
    [DisplayName("buildingid")]
    public string BuildingId { get; set; }
    
    [DisplayName("partner_id")]
    public string PartnerId { get; set; }
}