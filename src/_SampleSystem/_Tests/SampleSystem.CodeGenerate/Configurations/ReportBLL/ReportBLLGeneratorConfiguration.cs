using Framework.CustomReports.Generation.BLL;

using SampleSystem.CustomReports.Employee;

namespace SampleSystem.CodeGenerate
{
    public class ReportBLLGeneratorConfiguration : CustomReportBLLGeneratorConfiguration<ServerGenerationEnvironment>
    {
        public ReportBLLGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment, typeof(EmployeeReport).Assembly)
        {

        }
    }
}