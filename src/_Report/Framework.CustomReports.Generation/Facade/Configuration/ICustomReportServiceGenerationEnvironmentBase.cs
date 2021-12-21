using Framework.CustomReports.Generation.BLL;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.CustomReports.Generation.Facade
{
    public interface ICustomReportServiceGenerationEnvironmentBase : IGenerationEnvironmentBase, ICustomReportBLLGeneratorConfigurationContainer
    {
    }
}