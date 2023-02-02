using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Schedule.Interfaces.Helper.Parser;

public interface IParseXmlHelper : ISingletonDependency
{
    Task<string> ParsePolesseAsync();
}