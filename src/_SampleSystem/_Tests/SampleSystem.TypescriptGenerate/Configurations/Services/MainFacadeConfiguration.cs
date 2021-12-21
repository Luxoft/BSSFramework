using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.TypeScriptGenerate.Configurations.Environments;

namespace SampleSystem.TypeScriptGenerate.Configurations.Services
{
    public class MainFacadeConfiguration : BaseFacadeGenerationConfiguration<MainGenerationEnvironment>
    {
        internal MainFacadeConfiguration(MainGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override IEnumerable<Type> GetFacadeTypes()
        {
            var t = typeof(SampleSystem.WebApiCore.Controllers.Main.EmployeeController);

            return t.Assembly.GetTypes().Where(v => typeof(ControllerBase).IsAssignableFrom(v) && v.Namespace == t.Namespace);
        }

        public override IEnumerable<RequireJsModule> GetModules()
        {
            return base.GetModules()
                       .Concat(
                               new List<RequireJsModule>
                               {
                                   new RequireJsModule("* as dto", "../dto/entities.generated",
                                                       "SampleSystem.Generated.DTO",
                                                       "SampleSystem"),
                                   new RequireJsModule("* as persistent", "../../app/common/HierarchicalNode",
                                                       "Framework.Persistent")
                               });
        }

        public override string GetFacadeFileFactoryName()
        {
            return "Environment.current.context.facadeFactory";
        }
    }
}
