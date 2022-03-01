using Framework.CustomReports.Generation.BLL;

using WorkflowSampleSystem.CustomReports.Employee;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class ReportBLLGeneratorConfiguration : CustomReportBLLGeneratorConfiguration<ServerGenerationEnvironment>
    {
        public ReportBLLGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment, typeof(EmployeeReport).Assembly)
        {

        }
    }
}