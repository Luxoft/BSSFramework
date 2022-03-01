using Framework.CustomReports.Generation.Facade;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class CustomReportServiceGeneratorConfiguration : CustomReportServiceGeneratorConfiguration<ServerGenerationEnvironment>
    {
        public CustomReportServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {

        }
    }
}