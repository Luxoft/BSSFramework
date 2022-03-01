using Framework.CustomReports.Generation.Facade;

namespace SampleSystem.CodeGenerate
{
    public class CustomReportServiceGeneratorConfiguration : CustomReportServiceGeneratorConfiguration<ServerGenerationEnvironment>
    {
        public CustomReportServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {

        }
    }
}