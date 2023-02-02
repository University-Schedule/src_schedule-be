using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChoETL;
using Schedule.Dtos.Parser;
using Schedule.EntityFrameworkCore;
using Schedule.Inputs;
using Schedule.Interfaces;
using Schedule.Interfaces.Helper.Parser;
using Schedule.Models.Parser;
using Schedule.Settings;
// ReSharper disable All

namespace Schedule.Helpers.Parser;

public class ParseXmlHelper : IParseXmlHelper
{
    private readonly IScheduleSettingManagementProvider _scheduleSettingManagementProvider;
    private readonly IBuildingRepository _buildingRepository;
    private readonly ICardRepository _cardRepository;
    private readonly IClassRepository _classRepository;
    private readonly IClassRoomRepository _classRoomRepository;
    private readonly IDaysDefRepository _daysDefRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IPeriodRepository _periodRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IStudentSubjectsRepository _studentSubjectsRepository;
    private readonly ISubjectRepository _subjectRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ITermsDefRepository _termsDefRepository;
    private readonly IWeeksDefRepository _weeksDefRepository;
    private readonly PalesseDapperRepository _dapperRepository;
    private static Encoding unicode = Encoding.UTF8;

    public ParseXmlHelper(
        IScheduleSettingManagementProvider scheduleSettingManagementProvider, 
        IBuildingRepository buildingRepository, 
        ICardRepository cardRepository, 
        IClassRepository classRepository,
        IClassRoomRepository classRoomRepository, 
        IDaysDefRepository daysDefRepository, 
        IGradeRepository gradeRepository, 
        IGroupRepository groupRepository, 
        ILessonRepository lessonRepository, 
        IPeriodRepository periodRepository, 
        IStudentRepository studentRepository, 
        IStudentSubjectsRepository studentSubjectsRepository, 
        ISubjectRepository subjectRepository,
        ITeacherRepository teacherRepository, 
        ITermsDefRepository termsDefRepository, 
        IWeeksDefRepository weeksDefRepository, PalesseDapperRepository dapperRepository)
    {
        _scheduleSettingManagementProvider = scheduleSettingManagementProvider;
        _buildingRepository = buildingRepository;
        _cardRepository = cardRepository;
        _classRepository = classRepository;
        _classRoomRepository = classRoomRepository;
        _daysDefRepository = daysDefRepository;
        _gradeRepository = gradeRepository;
        _groupRepository = groupRepository;
        _lessonRepository = lessonRepository;
        _periodRepository = periodRepository;
        _studentRepository = studentRepository;
        _studentSubjectsRepository = studentSubjectsRepository;
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
        _termsDefRepository = termsDefRepository;
        _weeksDefRepository = weeksDefRepository;
        _dapperRepository = dapperRepository;
    }

    public async Task<string> ParsePolesseAsync()
    {
        var sw = new Stopwatch();
        sw.Start();
        
        var xmlStringResponse = await GetXmlStringFromResponseAsync(new DownLoadFileInput()
        {
            Url = await _scheduleSettingManagementProvider.GetPolesseApiUrlAsync(),
        });

        await RemoveCurrentDataAsync();

        await ParseBuildingsAsync(xmlStringResponse);
        await ParseCardAsync(xmlStringResponse);
        await ParseClassesAsync(xmlStringResponse);
        await ParseClassRoomsAsync(xmlStringResponse);
        await ParseDaysDefsAsync(xmlStringResponse);
        await ParseGradesAsync(xmlStringResponse);
        await ParseGroupsAsync(xmlStringResponse);
        await ParseLessonsAsync(xmlStringResponse);
        await ParsePeriodsAsync(xmlStringResponse);
        await ParseStudentsAsync(xmlStringResponse);
        await ParseStudentSubjectsAsync(xmlStringResponse);
        await ParseSubjectsAsync(xmlStringResponse);
        await ParseTeachersAsync(xmlStringResponse);
        await ParseTermsDefsAsync(xmlStringResponse);
        await ParseWeeksDefsAsync(xmlStringResponse);
        
        sw.Stop();

        return sw.Elapsed.ToString();
    }

    private static async Task<string> GetXmlStringFromResponseAsync(DownLoadFileInput input)
    {
        var requestHttp = new HttpRequestMessage(HttpMethod.Get, input.Url);

        using var client = new HttpClient();
        var response = await client.SendAsync(requestHttp);

        response.Content.Headers.ContentType.CharSet = "Windows-1251";
        return await response.Content.ReadAsStringAsync();
    }

    private async Task ParseBuildingsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<BuildingDto>.LoadText(xmlString).WithXPath("//building");

        var models =
            lstDto.Select(dto => new BuildingModel(dto.Id, dto.Name, dto.PartnerId));

        await _dapperRepository.InsertManyBuildingsAsync(models);
        //await _buildingRepository.InsertManyRecordsAsync(models);
    }

    private async Task ParseCardAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<CardDto>.LoadText(xmlString).WithXPath("//card");

        var models =
            lstDto.Select(dto => new CardModel(Guid.NewGuid().ToString(), dto.LessonId, dto.Period, dto.Days, dto.Weeks,
                dto.Terms, dto.ClassRoomIds));

        await _dapperRepository.InsertManyCardsAsync(models);
        //await _cardRepository.InsertManyAsync(models);
    }
    
    private async Task ParseClassesAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<ClassDto>.LoadText(xmlString).WithXPath("//class");

        var models =
            lstDto.Select(dto => new ClassModel(dto.Id, dto.Name, dto.Short, dto.ClassRoomIds,
                dto.TeacherId, dto.Grade, dto.PartnerId));

        await _dapperRepository.InsertManyClassesAsync(models);
        //await _classRepository.InsertManyAsync(models);
    }
    
    private async Task ParseClassRoomsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<ClassRoomDto>.LoadText(xmlString).WithXPath("//classroom");

        var models =
            lstDto.Select(dto => new ClassRoomModel(dto.Id, dto.Name, dto.Short, dto.Capacity,
                dto.BuildingId, dto.PartnerId));

        await _dapperRepository.InsertManyClassRoomsAsync(models);
        //await _classRoomRepository.InsertManyAsync(models);
    }
    
    private async Task ParseDaysDefsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<DaysdefDto>.LoadText(xmlString).WithXPath("//daysdef");

        var models =
            lstDto.Select(dto => new DaysDefModel(dto.Id, dto.Name, dto.Short, dto.Days));

        await _dapperRepository.InsertManyDaysDefsAsync(models);
        //await _daysDefRepository.InsertManyAsync(models);
    }
    
    private async Task ParseGradesAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<GradeDto>.LoadText(xmlString).WithXPath("//grade");

        var models = lstDto.Select(dto => new GradeModel(Guid.NewGuid().ToString(), dto.Grade, dto.Name, dto.Short));

        await _dapperRepository.InsertManyGradesAsync(models);
        //await _gradeRepository.InsertManyAsync(models);
    }
    
    private async Task ParseGroupsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<GroupDto>.LoadText(xmlString).WithXPath("//group");

        var models =
            lstDto.Select(dto => new GroupModel(dto.Id, dto.ClassId, dto.Name, dto.EntireClass,
                dto.DivisionTag, dto.StudentCount, dto.StudentIds));

        await _dapperRepository.InsertManyGroupsAsync(models);
        //await _groupRepository.InsertManyAsync(models);
    }
    
    private async Task ParseLessonsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<LessonDto>.LoadText(xmlString).WithXPath("//lesson");

        var models =
            lstDto.Select(dto => new LessonModel(dto.Id, dto.SubjectId, dto.ClassIds, dto.GroupIds,
                dto.TeacherIds, dto.ClassRoomIds, dto.PeriodsPerCard, dto.PeriodsPerWeek, dto.DaysDefId,
                dto.WeeksDefId, dto.TermsDefId, dto.SeminarGroup, dto.Capacity, dto.PartnerId));

        await _dapperRepository.InsertManyLessonsAsync(models);
        //await _lessonRepository.InsertManyAsync(models);
    }
    
    private async Task ParsePeriodsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<PeriodDto>.LoadText(xmlString).WithXPath("//period");

        var models =
            lstDto.Select(dto =>
                new PeriodModel(Guid.NewGuid().ToString(), dto.Period, dto.Short, dto.StartTime, dto.EndTime));

        await _dapperRepository.InsertManyPeriodsAync(models);
        //await _periodRepository.InsertManyAsync(models);
    }
    
    private async Task ParseStudentsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<StudentsDto>.LoadText(xmlString).WithXPath("//student");

        var models =
            lstDto.Select(dto => new StudentModel(dto.Id, dto.ClassId, dto.Name, dto.Number, dto.Email,
                dto.Mobile, dto.PartnerId, dto.FirstName, dto.LastName));

        await _dapperRepository.InsertManyStudentsAsync(models);
        //wait _studentRepository.InsertManyAsync(models);
    }
    
    private async Task ParseStudentSubjectsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<StudentSubjectsDto>.LoadText(xmlString).WithXPath("//studentsubject");

        var models =
            lstDto.Select(dto => new StudentSubjectsModel(Guid.NewGuid().ToString(), dto.StudentId, dto.SubjectId, dto.SeminarGroup,
                dto.Importance, dto.AlternateFor));

        await _dapperRepository.InsetManyStudentSubjectsAsync(models);
        //await _studentSubjectsRepository.InsertManyAsync(models);
    }
    
    private async Task ParseSubjectsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<SubjectDto>.LoadText(xmlString).WithXPath("//subject");

        var models =
            lstDto.Select(dto => new SubjectModel(dto.Id, dto.Name, dto.Short, dto.PartnerId));

        await _dapperRepository.InsertManySubjectsAsync(models);
        //await _subjectRepository.InsertManyAsync(models);
    }
    
    private async Task ParseTeachersAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<TeacherDto>.LoadText(xmlString).WithXPath("//teacher");

        var models =
            lstDto.Select(dto => new TeacherModel(dto.Id, dto.Name, dto.Short, dto.Gender, dto.Color,
                dto.Email, dto.Mobile, dto.PartnerId, dto.FirstName, dto.LastName));

        await _dapperRepository.InsertManyTeachersAsync(models);
        //await _teacherRepository.InsertManyAsync(models);
    }
    
    private async Task ParseTermsDefsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<TermsdefDto>.LoadText(xmlString).WithXPath("//termsdef");

        var models =
            lstDto.Select(dto => new TermsDefModel(dto.Id, dto.Name, dto.Short, dto.Terms));

        await _dapperRepository.InsertManyTermsDefsAsync(models);
        //await _termsDefRepository.InsertManyAsync(models);
    }
    
    private async Task ParseWeeksDefsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<WeeksdefDto>.LoadText(xmlString).WithXPath("//weeksdef");

        var models =
            lstDto.Select(dto => new WeeksDefModel(dto.Id, dto.Name, dto.Short, dto.Weeks));

        await _dapperRepository.InsertManyWeeksDefsAsync(models);
        //await _weeksDefRepository.InsertManyAsync(models);
    }

    private async Task RemoveCurrentDataAsync()
    {
        await _buildingRepository.DeleteAllRecordsAsync();
        await _cardRepository.DeleteAllRecordsAsync();
        await _classRepository.DeleteAllRecordsAsync();
        await _classRoomRepository.DeleteAllRecordsAsync();
        await _daysDefRepository.DeleteAllRecordsAsync();
        await _gradeRepository.DeleteAllRecordsAsync();
        await _groupRepository.DeleteAllRecordsAsync();
        await _lessonRepository.DeleteAllRecordsAsync();
        await _periodRepository.DeleteAllRecordsAsync();
        await _studentRepository.DeleteAllRecordsAsync();
        await _studentSubjectsRepository.DeleteAllRecordsAsync();
        await _subjectRepository.DeleteAllRecordsAsync();
        await _teacherRepository.DeleteAllRecordsAsync();
        await _termsDefRepository.DeleteAllRecordsAsync();
        await _weeksDefRepository.DeleteAllRecordsAsync();
        
        /*var buildings = await _buildingRepository.GetListAsync();

        if (buildings.Count > 0)
        {
            await _buildingRepository.DeleteManyAsync(buildings.Select(model => model.Id));
        }

        var cards = await _cardRepository.GetListAsync();

        if (cards.Count > 0)
        {
            await _cardRepository.DeleteManyAsync(cards.Select(model => model.Id));
        }

        var classes = await _classRepository.GetListAsync();

        if (classes.Count > 0)
        {
            await _classRepository.DeleteManyAsync(classes.Select(model => model.Id));
        }

        var classRooms = await _classRoomRepository.GetListAsync();

        if (classRooms.Count > 0)
        {
            await _classRoomRepository.DeleteManyAsync(classRooms.Select(models => models.Id));
        }

        var daysDefs = await _daysDefRepository.GetListAsync();

        if (daysDefs.Count > 0)
        {
            await _daysDefRepository.DeleteManyAsync(daysDefs.Select(models => models.Id));
        }

        var grades = await _gradeRepository.GetListAsync();

        if (grades.Count > 0)
        {
            await _gradeRepository.DeleteManyAsync(grades.Select(models => models.Id));
        }

        var groups = await _groupRepository.GetListAsync();

        if (groups.Count > 0)
        {
            await _groupRepository.DeleteManyAsync(groups.Select(models => models.Id));
        }

        var lessons = await _lessonRepository.GetListAsync();

        if (lessons.Count > 0)
        {
            await _lessonRepository.DeleteManyAsync(lessons.Select(models => models.Id));
        }

        var periods = await _periodRepository.GetListAsync();

        if (periods.Count > 0)
        {
            await _periodRepository.DeleteManyAsync(periods.Select(models => models.Id));
        }

        var students = await _studentRepository.GetListAsync();

        if (students.Count > 0)
        {
            await _studentRepository.DeleteManyAsync(students.Select(models => models.Id));
        }

        var studentsSubjects = await _studentSubjectsRepository.GetListAsync();

        if (studentsSubjects.Count > 0)
        {
            await _studentSubjectsRepository.DeleteManyAsync(studentsSubjects.Select(models => models.Id));
        }

        var subjects = await _subjectRepository.GetListAsync();

        if (subjects.Count > 0)
        {
            await _subjectRepository.DeleteManyAsync(subjects.Select(models => models.Id));
        }

        var teachers = await _teacherRepository.GetListAsync();

        if (teachers.Count > 0)
        {
            await _teacherRepository.DeleteManyAsync(teachers.Select(models => models.Id));
        }

        var termsDefs = await _termsDefRepository.GetListAsync();

        if (termsDefs.Count > 0)
        {
            await _termsDefRepository.DeleteManyAsync(termsDefs.Select(models => models.Id));
        }

        var weeksDefs = await _weeksDefRepository.GetListAsync();

        if (weeksDefs.Count > 0)
        {
            await _weeksDefRepository.DeleteManyAsync(weeksDefs.Select(models => models.Id));
        }*/
    }
}