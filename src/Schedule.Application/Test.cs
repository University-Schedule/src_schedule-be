using System.Threading.Tasks;
using Schedule.Helpers.Parser;
using Volo.Abp.Application.Services;

namespace Schedule;

public interface ITest : IApplicationService
{
    Task Testt();
}

public class Test : ApplicationService
{
    private readonly IParseXmlHelper _parseXmlHelper;

    public Test(IParseXmlHelper parseXmlHelper)
    {
       _parseXmlHelper = parseXmlHelper;
    }

    public async Task Testt()
    {
        await _parseXmlHelper.ParsePolesseAsync();
    }
}