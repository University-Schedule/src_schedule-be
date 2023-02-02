using System.Threading.Tasks;
using Schedule.Interfaces.Helper.Parser;
using Volo.Abp.Application.Services;
// ReSharper disable All

namespace Schedule;

public interface IParserAppService : IApplicationService
{
    Task<string> ParsePalesseSchedule();
}

public class ParserAppService : ApplicationService
{
    private readonly IParseXmlHelper _parseXmlHelper;

    public ParserAppService(IParseXmlHelper parseXmlHelper)
    {
       _parseXmlHelper = parseXmlHelper;
    }

    public async Task<string> ParsePalesseSchedule()
    {
        return await _parseXmlHelper.ParsePolesseAsync();
    }
}