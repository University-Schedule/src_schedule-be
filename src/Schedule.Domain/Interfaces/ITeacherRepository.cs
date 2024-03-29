﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Schedule.Models.Parser;
using Volo.Abp.Domain.Repositories;

namespace Schedule.Interfaces;

public interface ITeacherRepository : IRepository<TeacherModel, string>
{
    Task DeleteAllRecordsAsync();

    Task<List<string>> GetListTeachersByLetterAsync(string letter);

    Task<List<string>> GetListFirstLettersTeachersAsync();

    Task<bool> TeacherExistAsync(string teacher);
}