using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ChoETL;
using Schedule.Dtos.Parser;
using Schedule.Inputs;
using Schedule.Models.Parser;
using Schedule.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Schedule.Helpers.Parser;

public interface IParseXmlHelper : ISingletonDependency
{
    Task ParsePolesseAsync();
}

public class ParseXmlHelper : IParseXmlHelper
{
    private readonly IScheduleSettingManagementProvider _scheduleSettingManagementProvider;
    private readonly IRepository<BuildingModel, string> _buildingRepository;
    private readonly IRepository<CardModel, string> _cardRepository;
    private readonly IRepository<ClassModel, string> _classRepository;
    private readonly IRepository<ClassRoomModel, string> _classRoomRepository;
    private readonly IRepository<DaysDefModel, string> _daysDefRepository;
    private readonly IRepository<GradeModel, string> _gradeRoomRepository;
    private readonly IRepository<GroupModel, string> _groupRepository;
    private readonly IRepository<LessonModel, string> _lessonRepository;
    private readonly IRepository<PeriodModel, string> _periodRepository;
    private readonly IRepository<StudentModel, string> _studentRepository;
    private readonly IRepository<StudentSubjectsModel, string> _studentSubjectsRepository;
    private readonly IRepository<SubjectModel, string> _subjectRepository;
    private readonly IRepository<TeacherModel, string> _teacherRepository;
    private readonly IRepository<TermsDefModel, string> _termsDefRepository;
    private readonly IRepository<WeeksDefModel, string> _weeksDefRepository;

    public ParseXmlHelper(
        IScheduleSettingManagementProvider scheduleSettingManagementProvider, 
        IRepository<BuildingModel, string> buildingRepository, 
        IRepository<CardModel, string> cardRepository, 
        IRepository<ClassModel, string> classRepository,
        IRepository<ClassRoomModel, string> classRoomRepository, 
        IRepository<DaysDefModel, string> daysDefRepository, 
        IRepository<GradeModel, string> gradeRoomRepository, 
        IRepository<GroupModel, string> groupRepository, 
        IRepository<LessonModel, string> lessonRepository, 
        IRepository<PeriodModel, string> periodRepository, 
        IRepository<StudentModel, string> studentRepository, 
        IRepository<StudentSubjectsModel, string> studentSubjectsRepository, 
        IRepository<SubjectModel, string> subjectRepository,
        IRepository<TeacherModel, string> teacherRepository, 
        IRepository<TermsDefModel, string> termsDefRepository, 
        IRepository<WeeksDefModel, string> weeksDefRepository)
    {
        _scheduleSettingManagementProvider = scheduleSettingManagementProvider;
        _buildingRepository = buildingRepository;
        _cardRepository = cardRepository;
        _classRepository = classRepository;
        _classRoomRepository = classRoomRepository;
        _daysDefRepository = daysDefRepository;
        _gradeRoomRepository = gradeRoomRepository;
        _groupRepository = groupRepository;
        _lessonRepository = lessonRepository;
        _periodRepository = periodRepository;
        _studentRepository = studentRepository;
        _studentSubjectsRepository = studentSubjectsRepository;
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
        _termsDefRepository = termsDefRepository;
        _weeksDefRepository = weeksDefRepository;
    }

    public async Task ParsePolesseAsync()
    {
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
    }

    private static async Task<string> GetXmlStringFromResponseAsync(DownLoadFileInput input)
    {
        var requestHttp = new HttpRequestMessage(HttpMethod.Get, input.Url);

        using var client = new HttpClient();
        var response = await client.SendAsync(requestHttp);

        return await response.Content.ReadAsStringAsync();
    }

    private async Task ParseBuildingsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<BuildingDto>.LoadText(xmlString).WithXPath("//building");

        var models =
            lstDto.Select(dto => new BuildingModel(dto.Id, dto.Name, dto.PartnerId));

        await _buildingRepository.InsertManyAsync(models);
    }

    private async Task ParseCardAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<CardDto>.LoadText(xmlString).WithXPath("//card");

        var models =
            lstDto.Select(dto => new CardModel(dto.LessonId, dto.Period, dto.Days, dto.Weeks, dto.Terms,
                dto.ClassRoomIds));

        await _cardRepository.InsertManyAsync(models);
    }
    
    private async Task ParseClassesAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<ClassDto>.LoadText(xmlString).WithXPath("//class");

        var models =
            lstDto.Select(dto => new ClassModel(dto.Id, dto.Name, dto.Short, dto.ClassRoomIds,
                dto.TeacherId, dto.Grade, dto.PartnerId));

        await _classRepository.InsertManyAsync(models);
    }
    
    private async Task ParseClassRoomsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<ClassRoomDto>.LoadText(xmlString).WithXPath("//classroom");

        var models =
            lstDto.Select(dto => new ClassRoomModel(dto.Id, dto.Name, dto.Short, dto.Capacity,
                dto.BuildingId, dto.PartnerId));

        await _classRoomRepository.InsertManyAsync(models);
    }
    
    private async Task ParseDaysDefsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<DaysdefDto>.LoadText(xmlString).WithXPath("//daysdef");

        var models =
            lstDto.Select(dto => new DaysDefModel(dto.Id, dto.Name, dto.Short, dto.Days));

        await _daysDefRepository.InsertManyAsync(models);
    }
    
    private async Task ParseGradesAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<GradeDto>.LoadText(xmlString).WithXPath("//grade");

        var models =
            lstDto.Select(dto => new GradeModel(dto.Grade, dto.Name, dto.Short));

        await _gradeRoomRepository.InsertManyAsync(models);
    }
    
    private async Task ParseGroupsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<GroupDto>.LoadText(xmlString).WithXPath("//group");

        var models =
            lstDto.Select(dto => new GroupModel(dto.Id, dto.ClassId, dto.Name, dto.EntireClass,
                dto.DivisionTag, dto.StudentCount, dto.StudentIds));

        await _groupRepository.InsertManyAsync(models);
    }
    
    private async Task ParseLessonsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<LessonDto>.LoadText(xmlString).WithXPath("//lesson");

        var models =
            lstDto.Select(dto => new LessonModel(dto.Id, dto.SubjectId, dto.ClassIds, dto.GroupIds,
                dto.TeacherIds, dto.ClassRoomIds, dto.PeriodsPerCard, dto.PeriodsPerWeek, dto.DaysDefId,
                dto.WeeksDefId, dto.TermsDefId, dto.SeminarGroup, dto.Capacity, dto.PartnerId));

        await _lessonRepository.InsertManyAsync(models);
    }
    
    private async Task ParsePeriodsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<PeriodDto>.LoadText(xmlString).WithXPath("//period");

        var models =
            lstDto.Select(dto => new PeriodModel(dto.Period, dto.Short, dto.StartTime, dto.EndTime));

        await _periodRepository.InsertManyAsync(models);
    }
    
    private async Task ParseStudentsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<StudentsDto>.LoadText(xmlString).WithXPath("//student");

        var models =
            lstDto.Select(dto => new StudentModel(dto.Id, dto.ClassId, dto.Name, dto.Number, dto.Email,
                dto.Mobile, dto.PartnerId, dto.FirstName, dto.LastName));

        await _studentRepository.InsertManyAsync(models);
    }
    
    private async Task ParseStudentSubjectsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<StudentSubjectsDto>.LoadText(xmlString).WithXPath("//studentsubject");

        var models =
            lstDto.Select(dto => new StudentSubjectsModel(dto.StudentId, dto.SubjectId, dto.SeminarGroup,
                dto.Importance, dto.AlternateFor));

        await _studentSubjectsRepository.InsertManyAsync(models);
    }
    
    private async Task ParseSubjectsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<SubjectDto>.LoadText(xmlString).WithXPath("//subject");

        var models =
            lstDto.Select(dto => new SubjectModel(dto.Id, dto.Name, dto.Short, dto.PartnerId));

        await _subjectRepository.InsertManyAsync(models);
    }
    
    private async Task ParseTeachersAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<TeacherDto>.LoadText(xmlString).WithXPath("//teacher");

        var models =
            lstDto.Select(dto => new TeacherModel(dto.Id, dto.Name, dto.Short, dto.Gender, dto.Color,
                dto.Email, dto.Mobile, dto.PartnerId, dto.FirstName, dto.LastName));

        await _teacherRepository.InsertManyAsync(models);
    }
    
    private async Task ParseTermsDefsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<TermsdefDto>.LoadText(xmlString).WithXPath("//termsdef");

        var models =
            lstDto.Select(dto => new TermsDefModel(dto.Id, dto.Name, dto.Short, dto.Terms));

        await _termsDefRepository.InsertManyAsync(models);
    }
    
    private async Task ParseWeeksDefsAsync(string xmlString)
    {
        using var lstDto = ChoXmlReader<WeeksdefDto>.LoadText(xmlString).WithXPath("//weeksdef");

        var models =
            lstDto.Select(dto => new WeeksDefModel(dto.Id, dto.Name, dto.Short, dto.Weeks));

        await _weeksDefRepository.InsertManyAsync(models);
    }

    private async Task RemoveCurrentDataAsync()
    {
        var buildings = await _buildingRepository.GetListAsync();

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

        var grades = await _gradeRoomRepository.GetListAsync();

        if (grades.Count > 0)
        {
            await _gradeRoomRepository.DeleteManyAsync(grades.Select(models => models.Id));
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
        }
    }
}