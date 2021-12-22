using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

using SampleSystem.TypeScriptGenerate.Configurations.Environments;
using SampleSystem.WebApiCore.Controllers.CustomReport;
using SampleSystem.WebApiCore.Controllers.Report;

namespace SampleSystem.TypeScriptGenerate.Configurations.Services
{
    public class ReportFacadeGenerationConfiguration : BaseFacadeGenerationConfiguration<ConfigurationGenerationEnvironment>
    {
        internal ReportFacadeGenerationConfiguration(ConfigurationGenerationEnvironment environment)
            : base(environment)
        {
        }

        protected override ITypeScriptMethodPolicy CreateGeneratePolicy()
        {
            var policy = new TypeScriptMethodPolicyBuilder<SampleSystemGenericReportController>(true).Except(t => t.GetStream(default));

            return policy;
        }

        public override IEnumerable<RequireJsModule> GetModules()
        {
            return base.GetModules()
                       .Concat(
                               new List<RequireJsModule>
                               {
                                   new RequireJsModule(
                                                       "* as dto",
                                                       "luxite/report-generation/dto/all",
                                                       "Framework.Configuration",
                                                       "Framework.Configuration.Generated",
                                                       "Framework.DomainDriven.SerializeMetadata",
                                                       "Framework.Configuration.Generated.DTO"),
                                   new RequireJsModule(
                                                       "{ Stream }",
                                                       "../../mocked-system",
                                                       "System.IO.Stream"),
                               });
        }

        public override IEnumerable<Type> GetFacadeTypes()
        {
            yield return typeof(SampleSystemGenericReportController);
        }

        public override string GetGenericFacadeMethodInvocation(bool isPrimitiveType)
        {
            return isPrimitiveType ? "createReportSimpleService" : "createReportService";
        }
    }
}
