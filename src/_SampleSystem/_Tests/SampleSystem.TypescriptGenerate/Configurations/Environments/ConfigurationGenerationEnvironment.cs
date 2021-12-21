using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Configuration;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Generation.Domain;

using SampleSystem.TypeScriptGenerate.Configurations.DTO;
using SampleSystem.TypeScriptGenerate.Configurations.Services;
using SampleSystem.WebApiCore.Controllers;

using AuditPersistentDomainObjectBase = Framework.Configuration.Domain.AuditPersistentDomainObjectBase;
using DomainObjectBase = Framework.Configuration.Domain.DomainObjectBase;
using PersistentDomainObjectBase = Framework.Configuration.Domain.PersistentDomainObjectBase;

namespace SampleSystem.TypeScriptGenerate.Configurations.Environments
{

    public class ConfigurationGenerationEnvironment : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>,
                                                      Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase,
                                                      Framework.DomainDriven.DTOGenerator.TypeScript.ITypeScriptGenerationEnvironmentBase
    {
        public ConfigurationGenerationEnvironment()
            : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
        {
            this.ClientDTO = new ConfigurationDTOGeneratorConfiguration(this);

            this.ConfigurationFacade = new ConfigurationServiceFacadeConfiguration(this);

            this.ReportFacade = new ReportFacadeGenerationConfiguration(this);
        }

        public ConfigurationDTOGeneratorConfiguration ClientDTO { get; }

        public ConfigurationServiceFacadeConfiguration ConfigurationFacade { get; }

        public ReportFacadeGenerationConfiguration ReportFacade { get; }


        public override Type SecurityOperationCodeType { get; } = typeof(ConfigurationSecurityOperationCode);

        public override Type OperationContextType { get; } = typeof(ConfigurationOperationContext);

        protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
        {
            return base.GetDomainObjectAssemblies().Concat(new[] { typeof(ConfigurationSecurityOperationCode).Assembly });
        }
    }
}
