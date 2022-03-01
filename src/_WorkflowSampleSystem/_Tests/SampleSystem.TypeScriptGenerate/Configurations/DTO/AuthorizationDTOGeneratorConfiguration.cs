using System.Collections.Generic;

using Framework.Authorization;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.TypeScript;
using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

using Framework.DomainDriven.Generation.Domain;

using SampleSystem.TypeScriptGenerate.Configurations.Environments;
using SampleSystem.TypeScriptGenerate.Configurations.Services;

namespace SampleSystem.TypeScriptGenerate.Configurations.DTO
{
    public class AuthorizationDTOGeneratorConfiguration : TypeScriptDTOGeneratorConfiguration<AuthorizationGenerationEnvironment>
    {
        public AuthorizationDTOGeneratorConfiguration(AuthorizationGenerationEnvironment environment)
            : base(environment)
        {
        }

        protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
        {
            var facadePolicy = new DTOTypeScriptServiceGeneratePolicy<AuthorizationServiceFacadeConfiguration>(this.Environment.AuthFacade);

            return new TypeScriptDependencyGeneratePolicy(facadePolicy, this.GetTypeMaps())
                  .Add(typeof(AuthorizationSecurityOperationCode)); // Тип AuthorizationSecurityOperationCode нигде не протягивается по зависимостям из фасада, однако мы может указать его для генерации принудительно
        }
    }
}
