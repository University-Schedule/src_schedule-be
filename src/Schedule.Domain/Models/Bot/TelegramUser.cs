using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Schedule.Models.Bot;

[Table(ScheduleConsts.DbTablePrefix + "TelegramUsers")]
public class TelegramUser: CreationAuditedEntity<Guid>
{
    public long TelegramId { get; set; }
    
    public string UserName { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string LanguageCode { get; set; } 
    
    public bool? IsPremium { get; set; }
    
    public string CurrentStep { get; set; } 
    
    public string ScheduleGroup { get; set; }
    
    public bool IsTeacher { get; set; }
}