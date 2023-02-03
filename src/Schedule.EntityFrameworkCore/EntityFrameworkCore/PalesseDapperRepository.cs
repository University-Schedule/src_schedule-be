using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dapper;
using Schedule.Models.Parser;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace Schedule.EntityFrameworkCore;

[SuppressMessage("ReSharper", "IdentifierTypo")]
public class PalesseDapperRepository : DapperRepository<ScheduleDbContext>, ITransientDependency
{
  public PalesseDapperRepository(IDbContextProvider<ScheduleDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async Task InsertManyBuildingsAsync(IEnumerable<BuildingModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();

        const string query = "INSERT INTO [dbo].[AppBuildings] ([Id],[Name],[PartnerId],[CreationTime],[CreatorId]) " +
                             "VALUES " +
                             $"(@{nameof(BuildingModel.Id)}," +
                             $" @{nameof(BuildingModel.Name)}," +
                             $" @{nameof(BuildingModel.PartnerId)}," +
                             " GETUTCDATE()," +
                             " NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyCardsAsync(IEnumerable<CardModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();

        const string query = "INSERT INTO [dbo].[AppCards]([Id], [LessonId], [Period], [Days], [Weeks], [Terms], " +
                             "[ClassRoomIds], [CreationTime], [CreatorId])" +
                             " VALUES " +
                             $"(@{nameof(CardModel.Id)}, " +
                             $"@{nameof(CardModel.LessonId)}, " +
                             $"@{nameof(CardModel.Period)}, " +
                             $"@{nameof(CardModel.Days)}, " +
                             $"@{nameof(CardModel.Weeks)}, " +
                             $"@{nameof(CardModel.Terms)}, " +
                             $"@{nameof(CardModel.ClassRoomIds)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyClassesAsync(IEnumerable<ClassModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();

        const string query = $"INSERT INTO [dbo].[AppClasses] ([Id], [Name], [Short], [ClassRoomIds], [TeacherId], " +
                             $"[Grade], [PartnerId], [CreationTime], [CreatorId]) " +
                             $"VALUES " +
                             $"(@{nameof(ClassModel.Id)}, " +
                             $"@{nameof(ClassModel.Name)}, " +
                             $"@{nameof(ClassModel.Short)}, " +
                             $"@{nameof(ClassModel.ClassRoomIds)}, " +
                             $"@{nameof(ClassModel.TeacherId)}, " +
                             $"@{nameof(ClassModel.Grade)}, " +
                             $"@{nameof(ClassModel.PartnerId)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";

        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyClassRoomsAsync(IEnumerable<ClassRoomModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = $"INSERT INTO [dbo].[AppClassRooms] ([Id], [Name], [Short], [Capacity], [BuildingId], " +
                             $"[PartnerId], [CreationTime], [CreatorId]) " +
                             $"VALUES " +
                             $"(@{nameof(ClassRoomModel.Id)}, " +
                             $"@{nameof(ClassRoomModel.Name)}, " +
                             $"@{nameof(ClassRoomModel.Short)}, " +
                             $"@{nameof(ClassRoomModel.Capacity)}, " +
                             $"@{nameof(ClassRoomModel.BuildingId)}, " +
                             $"@{nameof(ClassRoomModel.PartnerId)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyDaysDefsAsync(IEnumerable<DaysDefModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppDaysDefs] ([Id], [Name], [Short], [Days], [CreationTime], [CreatorId])" +
                             " VALUES " +
                             $"(@{nameof(DaysDefModel.Id)}, " +
                             $"@{nameof(DaysDefModel.Name)}, " +
                             $"@{nameof(DaysDefModel.Short)}, " +
                             $"@{nameof(DaysDefModel.Days)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyGradesAsync(IEnumerable<GradeModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppGrades] ([Id], [Grade], [Name], [Short], [CreationTime], [CreatorId])" +
                             " VALUES " +
                             $"(@{nameof(GradeModel.Id)}, " +
                             $"@{nameof(GradeModel.Grade)}, " +
                             $"@{nameof(GradeModel.Name)}, " +
                             $"@{nameof(GradeModel.Short)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyGroupsAsync(IEnumerable<GroupModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppGroups] ([Id], [ClassId], [Name], [EntireClass], [DivisionTag], " +
                             "[StudentCount], [StudentIds], [CreationTime], [CreatorId]) " +
                             "VALUES " +
                             $"(@{nameof(GroupModel.Id)}, " +
                             $"@{nameof(GroupModel.ClassId)}, " +
                             $"@{nameof(GroupModel.Name)}, " +
                             $"@{nameof(GroupModel.EntireClass)}, " +
                             $"@{nameof(GroupModel.DivisionTag)}, " +
                             $"@{nameof(GroupModel.StudentCount)}, " +
                             $"@{nameof(GroupModel.StudentIds)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyLessonsAsync(IEnumerable<LessonModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppLessons] ([Id], [SubjectId], [ClassIds], [GroupIds], [TeacherIds], " +
                             "[ClassRoomIds], [PeriodsPerCard], [PeriodsPerWeek], [DaysDefId], [WeeksDefId], [TermsDefId], " +
                             "[SeminarGroup], [Capacity], [PartnerId], [CreationTime], [CreatorId])" +
                             " VALUES " +
                             $"(@{nameof(LessonModel.Id)}, " +
                             $"@{nameof(LessonModel.SubjectId)}, " +
                             $"@{nameof(LessonModel.ClassIds)}, " +
                             $"@{nameof(LessonModel.GroupIds)}, " +
                             $"@{nameof(LessonModel.TeacherIds)}, " +
                             $"@{nameof(LessonModel.ClassRoomIds)}, " +
                             $"@{nameof(LessonModel.PeriodsPerCard)}, " +
                             $"@{nameof(LessonModel.PeriodsPerWeek)}, " +
                             $"@{nameof(LessonModel.DaysDefId)}, " +
                             $"@{nameof(LessonModel.WeeksDefId)}, " +
                             $"@{nameof(LessonModel.TermsDefId)}, " +
                             $"@{nameof(LessonModel.SeminarGroup)}, " +
                             $"@{nameof(LessonModel.Capacity)}, " +
                             $"@{nameof(LessonModel.PartnerId)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyPeriodsAync(IEnumerable<PeriodModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppPeriods] ([Id], [Period], [Short], [StartTime], [EndTime], " +
                             "[CreationTime], [CreatorId])" +
                             " VALUES " +
                             $"(@{nameof(PeriodModel.Id)}, " +
                             $"@{nameof(PeriodModel.Period)}, " +
                             $"@{nameof(PeriodModel.Short)}, " +
                             $"@{nameof(PeriodModel.StartTime)}, " +
                             $"@{nameof(PeriodModel.EndTime)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyStudentsAsync(IEnumerable<StudentModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppStudents]([Id], [ClassId], [Name], [Number], [Email], " +
                             "[Mobile], [PartnerId], [FirstName], [LastName], [CreationTime], [CreatorId]) " +
                             "VALUES " +
                             $"({nameof(StudentModel.Id)}, " +
                             $"{nameof(StudentModel.ClassId)}, " +
                             $"{nameof(StudentModel.Name)}, " +
                             $"{nameof(StudentModel.Number)}, " +
                             $"{nameof(StudentModel.Email)}, " +
                             $"{nameof(StudentModel.Mobile)}, " +
                             $"{nameof(StudentModel.PartnerId)}, " +
                             $"{nameof(StudentModel.FirstName)}, " +
                             $"{nameof(StudentModel.LastName)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsetManyStudentSubjectsAsync(IEnumerable<StudentSubjectsModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppStudentSubjects] ([Id], [StudentId], [SubjectId], [SeminarGroup], " +
                             "[Importance], [AlternateFor], [CreationTime], [CreatorId]) " +
                             "VALUES " +
                             $"(@{nameof(StudentSubjectsModel.Id)}, " +
                             $"@{nameof(StudentSubjectsModel.StudentId)}, " +
                             $"@{nameof(StudentSubjectsModel.SubjectId)}, " +
                             $"@{nameof(StudentSubjectsModel.SeminarGroup)}, " +
                             $"@{nameof(StudentSubjectsModel.Importance)}, " +
                             $"@{nameof(StudentSubjectsModel.AlternateFor)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManySubjectsAsync(IEnumerable<SubjectModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppSubjects] ([Id], [Name], [Short], [PartnerId], [CreationTime], " +
                             "[CreatorId]) " +
                             "VALUES " +
                             $"(@{nameof(SubjectModel.Id)}, " +
                             $"@{nameof(SubjectModel.Name)}, " +
                             $"@{nameof(SubjectModel.Short)}, " +
                             $"@{nameof(SubjectModel.PartnerId)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyTeachersAsync(IEnumerable<TeacherModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppTeachers] ([Id], [Name], [Short], [Gender], [Color], [Email], [Mobile], " +
                             "[PartnerId], [FirstName], [LastName], [CreationTime], [CreatorId]) " +
                             "VALUES " +
                             $"(@{nameof(TeacherModel.Id)}, " +
                             $"@{nameof(TeacherModel.Name)}, " +
                             $"@{nameof(TeacherModel.Short)}, " +
                             $"@{nameof(TeacherModel.Gender)}, " +
                             $"@{nameof(TeacherModel.Color)}, " +
                             $"@{nameof(TeacherModel.Email)}, " +
                             $"@{nameof(TeacherModel.Mobile)}, " +
                             $"@{nameof(TeacherModel.PartnerId)}, " +
                             $"@{nameof(TeacherModel.FirstName)}, " +
                             $"@{nameof(TeacherModel.LastName)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyTermsDefsAsync(IEnumerable<TermsDefModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppTermsDefs] ([Id], [Name], [Short], [Terms], [CreationTime], [CreatorId]) " +
                             "VALUES " +
                             $"(@{nameof(TermsDefModel.Id)}, " +
                             $"@{nameof(TermsDefModel.Name)}, " +
                             $"@{nameof(TermsDefModel.Short)}, " +
                             $"@{nameof(TermsDefModel.Terms)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }

    public async Task InsertManyWeeksDefsAsync(IEnumerable<WeeksDefModel> models)
    {
        var dbConnection = await GetDbConnectionAsync();
        
        const string query = "INSERT INTO [dbo].[AppWeeksDefs] ([Id], [Name], [Short], [Weeks], [CreationTime], [CreatorId]) " +
                             "VALUES " +
                             $"(@{nameof(WeeksDefModel.Id)}, " +
                             $"@{nameof(WeeksDefModel.Name)}, " +
                             $"@{nameof(WeeksDefModel.Short)}, " +
                             $"@{nameof(WeeksDefModel.Weeks)}, " +
                             "GETUTCDATE(), " +
                             "NULL)";
        
        await dbConnection.ExecuteAsync(query, models, await GetDbTransactionAsync());
    }
}