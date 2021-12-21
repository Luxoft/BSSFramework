using System.Collections.Generic;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.CustomReports.Generation.BLL
{
    public class CustomReportBLLGenerator : CustomReportBLLGenerator<ICustomReportBLLGeneratorConfiguration<ICustomReportGenerationEnvironmentBase>>
    {
        public CustomReportBLLGenerator(ICustomReportBLLGeneratorConfiguration<ICustomReportGenerationEnvironmentBase> configuration)
            : base(configuration)
        {
        }
    }

    public class CustomReportBLLGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, ICustomReportBLLGeneratorConfiguration<ICustomReportGenerationEnvironmentBase>
    {
        public CustomReportBLLGenerator([NotNull] TConfiguration configuration)
            : base(configuration)
        {
        }

        protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
        {
            foreach (var link in this.Configuration.Links)
            {
                yield return new CustomReportBLLCodeFile<TConfiguration>(this.Configuration, link.CustomReportType, link.ParameterType);
            }
        }
    }
}
