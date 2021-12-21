using System;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator.TypeScript;
using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;

namespace SampleSystem.TypeScriptGenerate.Configurations.Services
{
    public abstract class BaseFacadeGenerationConfiguration<TEnvironment> : TypeScriptFacadeGeneratorConfiguration<TEnvironment>
       where TEnvironment : class, ITypeScriptGenerationEnvironmentBase
    {
        protected BaseFacadeGenerationConfiguration(TEnvironment environment)
            : base(environment)
        {
        }

        public override IEnumerable<RequireJsModule> GetModules()
        {
            var data = new List<RequireJsModule>
                       {
                           new RequireJsModule(
                                               "{ Guid, Convert, SimpleObject, SimpleDate, ObservableSimpleObject, ObservableSimpleDate }",
                                               "luxite/system",
                                               "System"),
                           new RequireJsModule("* as async", "luxite/async", string.Empty),
                           new RequireJsModule("{ OData }", "luxite/framework/odata", "Framework.OData"),
                           new RequireJsModule("{ Environment }", "luxite/environment"),
                           new RequireJsModule("{ Core }", "luxite/framework/framework")
                       };

            return data;
        }
    }
}
