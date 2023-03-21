using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.TypeScriptGenerate.Configurations.Environments;

namespace SampleSystem.TypeScriptGenerate.Configurations.Services;

public class ConfigurationServiceFacadeConfiguration : BaseFacadeGenerationConfiguration<ConfigurationGenerationEnvironment>
{
    internal ConfigurationServiceFacadeConfiguration(ConfigurationGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override IEnumerable<Type> GetFacadeTypes()
    {
        var t = typeof(Configuration.WebApi.Controllers.SystemConstantController);

        return t.Assembly.GetTypes().Where(v => typeof(ControllerBase).IsAssignableFrom(v) && v.Namespace == t.Namespace);
    }

    public override IEnumerable<RequireJsModule> GetModules()
    {
        return base.GetModules()
                   .Concat(
                           new List<RequireJsModule>
                           {
                                   new RequireJsModule(
                                                       "* as dto",
                                                       "../dto/configuration.generated",
                                                       "Framework.Configuration",
                                                       "Framework.Configuration.Generated",
                                                       "Framework.DomainDriven.SerializeMetadata",
                                                       "Framework.Configuration.Generated.DTO"),
                                   new RequireJsModule(
                                                       "* as mockdto",
                                                       "../../mocked-dto",
                                                       "Framework.Notification.DTO"),
                           });
    }
}
