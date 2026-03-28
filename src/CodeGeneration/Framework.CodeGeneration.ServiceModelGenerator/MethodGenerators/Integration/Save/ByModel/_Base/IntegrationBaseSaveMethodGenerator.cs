using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Integration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save.Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save.ByModel._Base;

public abstract class IntegrationBaseSaveMethodGenerator<TConfiguration> : IntegrationMethodGenerator<TConfiguration, BLLIntegrationSaveRoleAttribute>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly CodeTypeReference IdentTypeRef;

    protected readonly CodeTypeReference RichIntegrationTypeRef;


    protected IntegrationBaseSaveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.IdentTypeRef = this.Configuration
                                .Environment.ServerDTO
                                .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

        this.RichIntegrationTypeRef = this.Configuration
                                          .Environment.ServerDTO
                                          .GetCodeTypeReference(this.DomainType, DTOGenerator.Server.ServerFileType.RichIntegrationDTO);
    }
}
