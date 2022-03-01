using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Authorization;
using Framework.Authorization.Domain;
using Framework.DomainDriven.Generation.Domain;

using SampleSystem.TypeScriptGenerate.Configurations.DTO;
using SampleSystem.TypeScriptGenerate.Configurations.Services;

using AuditPersistentDomainObjectBase = Framework.Authorization.Domain.AuditPersistentDomainObjectBase;
using DomainObjectBase = Framework.Authorization.Domain.DomainObjectBase;
using PersistentDomainObjectBase = Framework.Authorization.Domain.PersistentDomainObjectBase;

namespace SampleSystem.TypeScriptGenerate.Configurations.Environments
{
    public class AuthorizationGenerationEnvironment : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>,
                                                      Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase,
                                                      Framework.DomainDriven.DTOGenerator.TypeScript.ITypeScriptGenerationEnvironmentBase
    {
        public AuthorizationGenerationEnvironment()
            : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
        {
            this.ClientDTO = new AuthorizationDTOGeneratorConfiguration(this);

            this.AuthFacade = new AuthorizationServiceFacadeConfiguration(this);
        }

        public AuthorizationDTOGeneratorConfiguration ClientDTO { get; }

        public AuthorizationServiceFacadeConfiguration AuthFacade { get; }

        public override Type SecurityOperationCodeType { get; } = typeof(AuthorizationSecurityOperationCode);

        public override Type OperationContextType { get; } = typeof(AuthorizationOperationContext);

        protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
        {
            return base.GetDomainObjectAssemblies().Concat(new[] { typeof(AuthorizationSecurityOperationCode).Assembly });
        }
    }
}
