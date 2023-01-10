﻿using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace Schedule.Models.Parser;

[Table(ScheduleConsts.DbTablePrefix + "Groups")]
public class GroupModel : Entity<string>
{
    public string Id { get; }
    public string ClassId { get; set; }
    public string Name { get; set; }
    public string EntireClass { get; set; }
    public string DivisionTag { get; set; }
    public string StudentCount { get; set; }
    public string StudentIds { get; set; }

    public GroupModel()
    {
        
    }

    public GroupModel(
        string id,
        string classId,
        string name,
        string entireClass,
        string divisionTag,
        string studentCount,
        string studentIds)
    {
        Id = id;
        ClassId = classId;
        Name = name;
        EntireClass = entireClass;
        DivisionTag = divisionTag;
        StudentCount = studentCount;
        StudentIds = studentIds;
    }
}